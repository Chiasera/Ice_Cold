using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float speed;
    public bool isGrounded;
    public bool isJumping;
    private bool jumpKeyPressed;
    public bool canJump;
    private float inputX;
    private float inputY;
    public bool isFalling;
    private bool isFacingRight;
    public bool isSprinting;
    SkeletonAnimation skeletonAnimation;
    float deltaButtonPressed;
    private string keyIndicator;
    private bool hasHitWall;
    private SkeletonRendererCustomMaterials materialRenderer;
    public Material[] materials;

    // Start is called before the first frame update
    void Start()
    {

        materialRenderer = GameObject.Find("BushWhiteFlowers_Outline").GetComponent<SkeletonRendererCustomMaterials>();
        speed = 7;
        skeletonAnimation = GameObject.Find("PlayerSkeleton").GetComponent<SkeletonAnimation>();
        rigidBody = GetComponent<Rigidbody2D>();
        canJump = true;
        isGrounded = true;
        isFacingRight = true;
        keyIndicator = "";

        //skeletonAnimation.CustomMaterialOverride.Add(materials[0], materials[1]);

    }

    // Update is called once per frame
    void Update()
    {

        
        /*================================================

         GETTING INPUTS FROM THE DIFFERENT AXIS

        ================================================*/

        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        jumpKeyPressed = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        bool D = Input.GetKeyDown(KeyCode.D);
        bool A = Input.GetKeyDown(KeyCode.A);
        bool left = Input.GetKeyDown(KeyCode.LeftArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);

        if (isSprinting) { speed = 13; } else speed = 7;

        if (D)
        {                     
            if (keyIndicator.Equals("D") && Time.time - deltaButtonPressed < 0.5f && inputX > 0)
            {
                isSprinting = true;
            }
            else isSprinting = false;
            keyIndicator = "D";
            deltaButtonPressed = Time.time;
        }
        if (A)
        {                      
            if (keyIndicator.Equals("A") && Time.time - deltaButtonPressed < 0.5f && inputX < 0)
            {
                isSprinting = true;
            }
            else isSprinting = false;
            keyIndicator = "A";
            deltaButtonPressed = Time.time;
        }

        if(isSprinting && inputX == 0)
        {
            isSprinting = !isSprinting;
        }


        /*================================================

         ANIMATOR

        ================================================*/



        if (inputX != 0 && !isJumping && !isSprinting)
        {
            skeletonAnimation.AnimationName = "Walking";
            skeletonAnimation.loop = true;
        }
        else if(isSprinting && !isJumping)
        {
            skeletonAnimation.AnimationName = "Running";
            skeletonAnimation.loop = true;
        } 
        else if(inputX == 0 || hasHitWall)
        {
            skeletonAnimation.AnimationName = "Idle";
            skeletonAnimation.loop = true;
        }

        if (isJumping)
        {
            skeletonAnimation.AnimationName = "Jumping";
            skeletonAnimation.loop = false;
        }
       

         /*================================================

         JUMP CONDITIONS, SETTING ALL OTHER CONDITIONS TO FALSE WHEN JUMPING

         ================================================*/

        if (canJump) //check if  player can jump
        {
            if (jumpKeyPressed && isGrounded)
            {
                Jump();
                isGrounded = false;
                isJumping = true;
            }
            canJump = false;
        }

        /*================================================

        IF THE PLAYER IS TOUCHING THE GROUND, IT CAN JUMP
        AGAIN AND MOVE FREELY. IF HE LEAVES THE MAINS GROUND
       IT MEANS HE MIGHT HAVE TAKEN A LADDER -- CAN

        ================================================*/

        if (isGrounded)
        {
            rigidBody.constraints = RigidbodyConstraints2D.None;
            isJumping = false;
            if (Input.GetKey(KeyCode.UpArrow) == false) // prevents the player from double jumping (for the moment)
            {
                canJump = true;
            }
        }

    }

    void FixedUpdate()  // FixedUpdate should be used when applying physics
    {

        HorizontalDisplacement();

        /*if (isClimbing)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            rigidBody.gravityScale = 0;
            VerticalDisplacement();
        }

        else
        {
            rigidBody.gravityScale = 1;

        }*/

        if (rigidBody.velocity.y < 0 && !isGrounded)
        {
            isGrounded = false;
            isFalling = true;
            canJump = false;
        }
        else
        {
            isFalling = false;
        }

        if(rigidBody.velocity.x < 0 && isFacingRight)
        {
            Flip();
            isFacingRight = !isFacingRight;

        } else if (rigidBody.velocity.x > 0 && !isFacingRight)
        {
            Flip();
            isFacingRight = !isFacingRight;
        }
    }

        /*================================================

         DISPLACEMENT, JUMP AND TRIGGER/COLLISION METHODS

         ================================================*/

    public void HorizontalDisplacement() //use in FixedUpdate only
    {
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        float Ymovement = rigidBody.velocity.y;
        float Xmovement = inputX;
        Vector2 displacement = new Vector2(Xmovement * speed, Ymovement);
        rigidBody.velocity = displacement; //transform.translate moves the object independently of its physical characteristics

    }

    public void VerticalDisplacement() //use in FixedUpdate only
    {
        float Xmovement = rigidBody.velocity.x;
        float Ymovement = inputY;
        Vector2 displacement = new Vector2(Xmovement, Ymovement * speed);
    }

    private void Jump()
    {
        rigidBody.AddForce(new Vector2(0, 35), ForceMode2D.Impulse);             
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;        
    }

    public void setGroundState(bool groundState)
    {
        isGrounded = groundState;
    }

    public void setColliderState(bool hitWall)
    {
        hasHitWall = hitWall;
    }
}

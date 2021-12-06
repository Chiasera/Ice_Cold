using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCollider : MonoBehaviour
{
    private Collider2D edgeCollider;
    private PlayerController player;
    private 
    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = this.GetComponent<Collider2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    // Start is called before the first frame update
    Animator anim;
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetTrigger("Open");
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    
    }
}

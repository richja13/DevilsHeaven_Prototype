using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    Vector2 startingpos;
    public int i = 1; 
    // Start is called before the first frame update
    void Start()
    {
        startingpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * i * Time.fixedDeltaTime);

        if(transform.position.x >= startingpos.x + 10)
        {
            MoveLeft();
        }
        else if(transform.position.x <= startingpos.x - 10)
        {
            
            MoveRight();
        }
    }

    void MoveLeft()
    {
        i = -1;
      
    }

    void MoveRight()
    {
        i = 1;
        
    }
}

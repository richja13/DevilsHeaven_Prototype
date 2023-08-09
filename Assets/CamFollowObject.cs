using System.Net.Http.Headers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamFollowObject : MonoBehaviour
{
    public GameObject Target;
    float Y;
    float X;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    async void Update()
    {

        X = transform.position.x;
        Y = transform.position.y;
        // Mathf.Lerp(Target.transform.position.y, transform.position.y, 0.4f);
/*if(Target.GetComponent<Rigidbody2D>().velocity.y > 0.1)
        {
            Y = Mathf.Lerp(transform.position.y, Target.transform.position.y, 0.20f);
        }
        else
        {
            Y = Mathf.Lerp(Target.transform.position.y, transform.position.y, 1.5f);
        }
  */      

        if(Target.transform.eulerAngles.y == 180)
        {
            transform.position = new Vector3(X - 5f, Y,-10); 
            //Vector3.Lerp(new Vector3(transform.position.x, transform.position.y,  -10), new Vector3(Target.transform.position.x, Target.transform.position.y + 3.70f,  -10), 0.05f);
        }
        else
        {
            transform.position = new Vector3(X + 5f, Y,-10); 
             //Vector3.Lerp(new Vector3(transform.position.x, transform.position.y,  -10), new Vector3(Target.transform.position.x, Target.transform.position.y + 3.70f,  -10), 0.05f);
        }

       // Application.targetFrameRate = 60;

        

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

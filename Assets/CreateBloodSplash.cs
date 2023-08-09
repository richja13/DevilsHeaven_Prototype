using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBloodSplash : MonoBehaviour
{
    public GameObject BloodSplash;
    
   

    public void CreateBlood()
    {
        Instantiate(BloodSplash, new Vector2(transform.position.x, transform.position.y - 1.4f), transform.transform.rotation);
    }
}

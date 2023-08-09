using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivatorScript : MonoBehaviour
{
    bool InRoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InRoom)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(InRoom)
            {
                InRoom = false;
            }
            else
            {
                InRoom = true;
            }
        }
    }

    IEnumerator ChangeColor()
    {


        yield return new WaitForSeconds(0.5f);
    }
}

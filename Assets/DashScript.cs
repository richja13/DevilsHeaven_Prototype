using System.Net.Sockets;

using System.Net.Mime;

using System.Threading;

using System.Numerics;

using System.Net.Http.Headers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using EZCameraShake;

public class DashScript : MonoBehaviour
{
    Animator anim;
    public Animator MainShadowAnim;
    Rigidbody2D rb;
    public GameObject Shadow1;
    public GameObject Shadow2;
    public Transform Shadow1Pos;
    public Transform Shadow2Pos;
    [SerializeField]  VolumeProfile  mVolumeProfile;
    public Vignette mVignette;
    public float inte;
    float DelayTimer;
    float DashTimer;
    bool CanDash = true;
    public GameObject DashDustParticles;
    public Transform DashDustPos;
    // Start is called before the first frame update
    void Start()
    {
        inte = 0.288f;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

          for (int i = 0; i < mVolumeProfile.components.Count; i++)
      {
        if (mVolumeProfile.components[i].name == "Vignette")
        {
            mVignette = (Vignette)mVolumeProfile.components[i];
        }
      }
    }
    
    // Update is called once per frame
    void Update()
    {
        ClampedFloatParameter intensity =  mVignette.intensity;
        intensity.value = inte;

        if(Input.GetMouseButtonDown(1))
        {
            CanDash = true;
                 anim.SetBool("Dash", true);
        MainShadowAnim.SetTrigger("Dash");
        Instantiate(Shadow1, Shadow1Pos.position, transform.rotation,this.gameObject.transform);
        Instantiate(Shadow2, Shadow2Pos.position, transform.rotation,this.gameObject.transform);
        Instantiate(DashDustParticles, DashDustPos.position, transform.rotation);
        inte = 0.43f;
          DashTimer = 1.7f;
        }

        Dash();
    }

    void Dash()
    {
        if(CanDash)
        {
            CinemaMachineShake.Instance.ShakeCamera(2,.4f);
          
            DelayTimer = 20f;
            if(DashTimer > 0)
            {
                PlayerController.CanRotate = false;
                DashTimer -= 4 * Time.fixedDeltaTime;
                if (transform.eulerAngles.y == 180)
                { 
                    rb.velocity = new UnityEngine.Vector2(-5800 * Time.deltaTime,0);
                    //transform.position = UnityEngine.Vector2.MoveTowards(transform.position, new UnityEngine.Vector2(transform.position.x - 1f, transform.position.y), 30f * Time.fixedDeltaTime);
                }
                else
                {
                    rb.velocity = new UnityEngine.Vector2(5800 * Time.deltaTime,0);
                    //transform.position = UnityEngine.Vector2.MoveTowards(transform.position, new UnityEngine.Vector2(transform.position.x + 1f, transform.position.y), 30f * Time.fixedDeltaTime);
                }
            }
            else
            {
                CanDash = false;
            }
        }
        

    }

    /*bool CanDash()
    {
        if(DelayTimer > 0)
        {
            DelayTimer -= 6f * Time.fixedDeltaTime;
            return false;
        }
        else
        {
            DelayTimer = 0;
            return true;
        }
    }
    */
}

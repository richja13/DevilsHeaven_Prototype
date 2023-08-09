
using System.Net.Http.Headers;
using System.Linq.Expressions;
//using System.Reflection.PortableExecutable;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    private float gravity;
    private Rigidbody2D rb;
    private Animator anim;
    public bool CanMove = true;
    public static bool CanRotate = true;
    public bool grounded;
    private GameObject currentOneWayPlatform;
    private CircleCollider2D playerCollider;
    public GameObject BloodParticles;
    public GameObject RunningParticles;
    public GameObject JumpParticles;
    public Transform FootStepPos;
    public static PlayerController Instance {get; private set;}
    public BoxCollider2D boxCollider2D;
    public LayerMask GroundLayer;

    float jumpTimeCounter;
    public float jumpTime;
    public float jumpForce;
    protected bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
        playerCollider = GetComponent<CircleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        WallsCollider();
        //Testowe
        if(rb.velocity.y > 0 && !grounded)
        {
            anim.SetBool("Jump",true);
        }
        else if(!grounded && rb.velocity.y < 0)
        {
            anim.SetBool("Jump",false);
            anim.SetBool("Fall",true);
        }
        else if(grounded)
        {
            anim.SetBool("Fall",false);            
        }
        
       

        //CheckIfGrounded();
       
        
        if(CanMove)
        {
            Move();
            Jump();
        }
        else
        {
            Invoke("CheckIfCanMove", 0.6f);
        }
        
    }

    public void CreateRunningParticles()
    {
        Instantiate(RunningParticles,FootStepPos.transform.position,transform.rotation);
    }


    private void IsGrounded()
    {
        float extraHeightText = 1f;
        RaycastHit2D raycastiHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down,boxCollider2D.bounds.extents.y + extraHeightText, GroundLayer);
        
        Color rayColor;
        if(raycastiHit.collider == null)
        {
            rayColor = Color.green;
            grounded = false;
           
            Debug.Log("PlayerNieWykryto");
        }
        else 
        {
            rayColor = Color.red;
            grounded = true;
            Debug.Log(raycastiHit.collider. name);
        }

      
        Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        //return raycastiHit.collider != null;
    }


    RaycastHit2D raycastHit;
    bool WallsCollider()
    {
        float extraHeightText = 1.1f;
        if(PlayerHp.Instance.direction == 1)
        {
            raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.right,boxCollider2D.bounds.extents.x + extraHeightText, GroundLayer);
        }
        else
        {
            raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.left,boxCollider2D.bounds.extents.x + extraHeightText, GroundLayer);
        }
        
        Color rayColor;
        if(raycastHit.collider == null)
        {
            rayColor = Color.green;
            return false;
           
           // Debug.Log("PlayerNieWykryto");
        }
        else 
        {
            rayColor = Color.red;
            rb.velocity = new Vector2(-0.1f, rb.velocity.y);
           // CanMove = false;
           return true;
           // Debug.Log(raycastiHit.collider. name);
        }

        if(PlayerHp.Instance.direction == 1)
        {
            Debug.DrawRay(boxCollider2D.bounds.center, Vector2.right * (boxCollider2D.bounds.extents.x + extraHeightText), rayColor);
        }
        else
        {
            Debug.DrawRay(boxCollider2D.bounds.center, Vector2.left * (boxCollider2D.bounds.extents.x + extraHeightText), rayColor);
        }
        //return raycastiHit.collider != null;
    }

    // void CheckIfGrounded()
    // {
    //     if(grounded)
    //     {
    //         rb.gravityScale = 0;
    //     }
    //     else
    //     {
    //         rb.gravityScale = gravity;
    //     }
    // }

    void OnCollisionStay2D(Collision2D collision)
    {
        // if(collision.gameObject.tag == "ground")
        // {
        //     grounded = true;
        // }
        // else
        // {
        //    grounded = false;
        // }        
    }

   

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground" || collision.gameObject.tag == "OneWayPlatform")
        {
            grounded = true;
          
        }

        if(collision.gameObject.tag == "OneWayPlatform")
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "ground" || collision.gameObject.tag == "OneWayPlatform")
        {
            grounded = false;
        }
        
        if(collision.gameObject.tag == "OneWayPlatform")
        {
            currentOneWayPlatform = null;
        }
    }


    private IEnumerator DisableCollision()
    {
        TilemapCollider2D platformCollider = currentOneWayPlatform.GetComponent<TilemapCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    void Move()
    {
       
        float MoveSpeed = 1100f ; 

        if(Input.GetKey(KeyCode.D))
        {
            //transform.Translate(MoveSpeed * Time.deltaTime,0,0);

            if(!WallsCollider())
            {
                rb.velocity = new Vector2(1 * MoveSpeed * Time.fixedDeltaTime , rb.velocity.y) ;
            }
            
            if(CanRotate)
            {
                transform.rotation = new Quaternion(0, 0, 0,0);
            }

            anim.SetBool("Running", true);
        }   

        
        if(Input.GetKey(KeyCode.A))
        {
            if(!WallsCollider())
            {
                rb.velocity = new Vector2(1 * -MoveSpeed * Time.fixedDeltaTime , rb.velocity.y);
            }
            //transform.Translate(MoveSpeed * Time.deltaTime,0,0);
            if(CanRotate)
            {
                transform.rotation = new Quaternion(0, 180, 0,0);
            }
            anim.SetBool("Running", true);
        }    

        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Running", false);
        }

        if(Input.GetKey(KeyCode.S))
        {
           if(currentOneWayPlatform != null)
           {
               StartCoroutine(DisableCollision());
           }
        }  
    }

    void CheckIfCanMove()
    {
        CanMove = true;
    }

   void Jump()
    {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) && grounded)
            {
                
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rb.velocity = new Vector2(rb.velocity.x, 1 * jumpForce);
                Instantiate(JumpParticles,new Vector2(transform.position.x, transform.position.y - 0.8f),transform.rotation);
                anim.SetBool("Jump",true);
                
            }
        
            if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) && isJumping )
            {                           
                if(jumpTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 1 * jumpForce);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    anim.SetBool("Jump",false);
                    anim.SetBool("Falling",true);
                   
                }
            }

            if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
            {
                isJumping = false;
                anim.SetBool("Jump",false);
                anim.SetBool("Falling",true);
              
            }
        }

    // void Jump()
    // {
    //     float JumpForce = 1620000f;
    //     if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && grounded)
    //     {
    //         rb.AddForce(Vector2.up * JumpForce * Time.fixedDeltaTime, ForceMode2D.Force);
    //         Instantiate(JumpParticles,new Vector2(transform.position.x, transform.position.y - 0.8f),transform.rotation);
    //     }
    // }

    void ChangeDirection()
    {
              
         //Get the Screen positions of the object
         Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
         
         //Get the Screen position of the mouse
         Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
         
         //Get the angle between the points
         float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
 
         
        if(angle > 90 && angle < - 90)
        {
           
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 2, transform.localScale.y);
        }

         //transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle));
     }
 
        float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
         return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void KnockBack()
    {
        CanMove = false;
        if(PlayerHp.Instance.direction == 1)
        {
            rb.velocity = new Vector2(1f,3f) * new Vector2(rb.velocity.x + 12f, 6f);
        }
        else
        {
            rb.velocity = new Vector2(-1f,3f) * new Vector2(rb.velocity.x + 12f, 6f);
        }
        //Debug.Log("ODRZUT");
    }

    public void PlayBlood()
    {
        Instantiate(BloodParticles, new Vector2(transform.position.x, transform.position.y), transform.rotation);
    }

    public void ChangeCanRotate()
    {
        CanRotate = true;
        anim.SetBool("Hit", false);
        CanMove = true;
    }

    public void GetHit()
    {
        anim.SetBool("Hit", true);
        CanMove = false;
        CanRotate = false;
        KnockBack();
    }
}

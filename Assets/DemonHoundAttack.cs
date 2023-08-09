using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHoundAttack : MonoBehaviour
{
    Rigidbody2D rb2D;
    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;
    public LayerMask WallsLayer;
    private GameObject Player;
    public float AttackTimer;
    UnityEngine.Vector2 RaycastVector;
    UnityEngine.Vector2 AttackVector;
    bool CanAttack = true;
    Vector2 Pos;
    public float Speed;
    public float AttackDmg;
    public float attackingSpeed;
    public GameObject BloodParticles;
    public float RandomNumber;
    bool CanWalk = true;
    bool CanTurn = true;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        RaycastVector = UnityEngine.Vector2.left;
        AttackVector = new UnityEngine.Vector2(-100, 100);
    }

    void FacePlayer()
    {
        if (transform.position.x > Player.transform.position.x)
        {
            AttackVector = new UnityEngine.Vector2(-100, 70);
            transform.eulerAngles = new UnityEngine.Vector2(0, 0);
            RaycastVector = -UnityEngine.Vector2.right;
        }
        else
        {
           AttackVector = new UnityEngine.Vector2(100, 70);
            transform.eulerAngles = new UnityEngine.Vector2(0, 180);
            RaycastVector = -UnityEngine.Vector2.left;
        }
    }
    void FixedUpdate()
    {
      
    
        if (PlayerDetected())
        {
            AttackSystem();
            

            CanTurn = false;
          
        }

        FacePlayer();
        /* else 
         {
             if (CanWalk)
             {
                 Pos = transform.position;
                 CanWalk = false;
             }

             WalkingSystem(Pos);
         }*/
    }


    void WalkingSystem(Vector2 Pos)
    {

        if (IsGrounded() && DetectWall())
        {
            if (Vector2.Distance(transform.position, Pos) > 8)
            {
                StartCoroutine(Turn());
                CanTurn = true;
             
            }
            else
            {
                Walk();
            }
        }
        else
        {
            StartCoroutine(Turn());
            CanTurn = true;
           
        }
    }

    void Walk()
    {
        rb2D.velocity = -transform.right * Time.fixedDeltaTime * Speed * 10;
        anim.SetBool("Running", true);
    }
    
    IEnumerator Turn()
    {
       
        yield return new WaitForSeconds(2f);
        if (CanTurn)
        {
            
            if (transform.eulerAngles.y == 0f)
            {
                transform.eulerAngles = new Vector2(0, 180f);
                RaycastVector = Vector2.right;
            }
            else if (transform.eulerAngles.y == 180f)
            {
                transform.eulerAngles = new Vector2(0, 0f);
                RaycastVector = -Vector2.right;
            }

            CanTurn = false;
        }
       
        Stop();
       
    }

    void Stop()
    {
        CanWalk = true;
    }

    bool IsGrounded()
    {
        Vector2 position = new Vector2(transform.position.x + 2f * RaycastVector.x, transform.position.y);
        Vector2 direction = Vector2.down;
        float distance = 1.3f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, GroundLayer);
        UnityEngine.Debug.DrawRay(position, direction, Color.green);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    public void AttackSystem()
    {
       
        FacePlayer();
        AttackDelay();
        if (CanAttack)
        {
            Debug.Log("GAZOLINA");
            RandomNumber = Random.Range(0, 100);

            if (RandomNumber > 80 && PlayerDistance() > 7)
            {
                ClawsAttack();
                anim.SetBool("Running", false);
            }
            else
            {
                MoveToPlayer();
            }
        }
    }

    public float PlayerDistance()
    {
        return UnityEngine.Vector2.Distance(transform.position, Player.transform.position);
    }

    public void MoveToPlayer()
    {

        if (PlayerDetected())
        {
           

            if (PlayerDistance() > 3.5)
            {
                rb2D.velocity = -transform.right * Time.fixedDeltaTime * Speed * 10;
                anim.SetBool("Running", true);
            }
            else if (CanAttack)
            {

                anim.SetBool("Running", false);
                anim.SetTrigger("Attack");
               
            }
        }
    }


    public void BasicAttack()
    {

        if (PlayerDistance() < 3.5)
        {
            Player.GetComponent<PlayerController>().PlayBlood();
            Player.GetComponent<PlayerHp>().GetDamage(AttackDmg);
        }
    }

    public void BasicAttack2()
    {
        if (PlayerDistance() < 3.5)
        {
            Player.GetComponent<PlayerController>().PlayBlood();
            Player.GetComponent<PlayerHp>().GetDamage(AttackDmg);
            CanAttack = false;
            AttackTimer = -5f;
        }
    }

    public bool PlayerDetected()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float distance = 12f;
        RaycastHit2D hit = Physics2D.Raycast(position, RaycastVector, distance,PlayerLayer);
        UnityEngine.Debug.DrawRay(position, RaycastVector, Color.red,distance);

        if (hit.collider == null)
        {
          //  Debug.Log("NIE WYKRYTY");
        }
        else
        {
           // Debug.Log(hit.collider.name);
        }

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DetectWall()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float distance = 2f;
        RaycastHit2D hit = Physics2D.Raycast(position, RaycastVector, distance, WallsLayer);
        UnityEngine.Debug.DrawRay(position, RaycastVector, Color.red, distance);

        if (hit.collider != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    private void AttackDelay()
    {
        if (AttackTimer < 0)
        {
            AttackTimer += attackingSpeed;
        }
        else
        {
            CanAttack = true;
        }
    }

    private void ClawsAttack()
    {
        AttackTimer = -10;
        rb2D.AddForce(AttackVector, ForceMode2D.Impulse);
        //UnityEngine.Debug.Log("ClawsAttack");
        CanAttack = false;
    }
}

using System.Numerics;
using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
   
    Rigidbody2D rb2D;
    public LayerMask PlayerLayer;
    private GameObject Player;
    private float AttackTimer;
    UnityEngine.Vector2 RaycastVector;
    UnityEngine.Vector2 AttackVector;
    bool CanAttack = true;

    public float Speed;
    public float AttackDmg;
    public float attackingSpeed;
    public GameObject BloodParticles;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        RaycastVector = UnityEngine.Vector2.left;
        AttackVector = new UnityEngine.Vector2(-1000, 1000);
    }

    void FacePlayer()
    {
        if(transform.position.x > Player.transform.position.x)
        {
            transform.eulerAngles = new UnityEngine.Vector2(0,0);
            RaycastVector = UnityEngine.Vector2.left;
            AttackVector = new UnityEngine.Vector2(-1, 1) * 50;
        }
        else
        {
            transform.eulerAngles = new UnityEngine.Vector2(0, 180);
            AttackVector = new UnityEngine.Vector2(1, 1) * 50;
            RaycastVector = -UnityEngine.Vector2.left;
        }
    }

    void FixedUpdate()
    {
        FacePlayer();
    }

    public float PlayerDistance()
    {
        return UnityEngine.Vector2.Distance(transform.position, Player.transform.position);
    }



    public void BasicAttack()
    {
        AttackTimer = - 45f;
        CanAttack = false;
        PlayerController.Instance.PlayBlood();
    }

    public bool PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, RaycastVector,15 , PlayerLayer);
        UnityEngine.Debug.DrawRay(transform.position, RaycastVector, Color.green, 15, false);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void AttackDelay()
    {
        if(AttackTimer < 0)
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
        AttackTimer = -30;
        rb2D.AddForce(AttackVector, ForceMode2D.Impulse);
        UnityEngine.Debug.Log("Attack");
        CanAttack = false;
    }

    public void PlayBlood()
    {
        Instantiate(BloodParticles, transform.position, transform.rotation);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerAttack : MonoBehaviour
{
 
    bool AttackReady;
    public float basicAttackDelay;
    private float AttackDelay;
    public LayerMask EnemyLayer;
    public LayerMask DestroyableLayer;
    Animator anim;
    public Transform attackPoint;
    public float attackRange;
    public float WeaponDamage;
    public float KnockbackForce;
    Rigidbody2D rb;
    public int attackCounter;
    public Animator AttackVFXAnimator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AttackDelay = basicAttackDelay;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Delay();

        if(Input.GetMouseButtonDown(0) && AttackReady)
        {     
            PlayerController.Instance.CanMove = false;
           
            anim.SetBool("Running", false);
            AttackReady = false;

            if (attackCounter == 0 && !anim.GetBool("Attack2"))
            {
                anim.SetBool("Attack", true);
            }
            else if(attackCounter >= 1 && !anim.GetBool("Attack"))
            {
                anim.SetBool("Attack2", true);
                attackCounter = 0;
            }
        }
    }

    public void Attack1VFX()
    {
        AttackVFXAnimator.SetTrigger("Attack1");
    }

    public void Attack2VFX()
    {
        AttackVFXAnimator.SetTrigger("Attack2");
    }

    public void MeleeAttack()
    {
       
        CinemaMachineShake.Instance.ShakeCamera(1f,.2f);
        rb.AddForce(transform.right * 20000 * Time.deltaTime, ForceMode2D.Impulse);
      
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().GettDamage(WeaponDamage, KnockbackForce);
            enemy.GetComponent<EnemyMovement>().PlayBlood();
            this.gameObject.GetComponent<PlayerHp>().AddHealPotion(2f);
        }

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, DestroyableLayer);
        
        foreach (Collider2D destroyable in hitObjects)
        {
            destroyable.GetComponent<Animator>().SetTrigger("Destroy");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Delay()
    {
        if (!AttackReady)
        {
            if (AttackDelay > 0)
            {
                AttackDelay -= 2f * Time.fixedDeltaTime;
            }
            else
            {
                AttackDelay = basicAttackDelay;
                AttackReady = true;
            }
        }
    }
}

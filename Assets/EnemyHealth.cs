using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyHealth : MonoBehaviour
{
    public float MaxHealth;
    private float Health;
    public GameObject HealthBar;
    private float HealthBarWidth;
    Animator anim;
    public RuntimeAnimatorController stunAnimatorController;
    public RuntimeAnimatorController NormalAnimatorController;
    public float stunTime;
    public Material HitMaterial;
    Material NormalMaterial;
    int direction;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        Health = MaxHealth;
        NormalMaterial = this.gameObject.GetComponent<SpriteRenderer>().material;
      
    }

    // Update is called once per frame
    void Update()
    {
        Direction();
        //HealthBarWidth = (Health)/MaxHealth;
        //HealthBar.transform.localScale = new Vector3(HealthBarWidth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.y);
        if(Death())
        {
            this.gameObject.GetComponent<EnemyMovement>().enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        }

        if (this.gameObject.GetComponent<SpriteRenderer>().material != NormalMaterial)
        {
            Invoke("ChangeMaterial", 0.2f);
        }
    }

    public void GettDamage(float Dmg, float knockbackForce)
    {
        if (!Death())
        {
            if(Health > 0)
            {
                anim.runtimeAnimatorController = stunAnimatorController;
            }

            anim.SetTrigger("Hit");
            Health -= Dmg;
            this.gameObject.GetComponent<EnemyMovement>().enabled = false;

            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(300 * direction, 500) * knockbackForce * Time.fixedDeltaTime;

            if (this.gameObject.GetComponent<EnemyMovement>().enabled == false)
            {
              
                Invoke("Stun", stunTime);
                
            }
            this.gameObject.GetComponent<SpriteRenderer>().material = HitMaterial;
        }
    }

    public void ChangeMaterial()
    {
        this.gameObject.GetComponent<SpriteRenderer>().material = NormalMaterial;
    }

    public void Stun()
    {
        GetComponent<EnemyMovement>().enabled = true;

    }

    public bool Death()
    {
        if (Health <= 0)
        {
             anim.runtimeAnimatorController = NormalAnimatorController;
            anim.SetBool("Death", true);
            return true;
        }
        else
        {
            return false;
        }
    }

    void Direction()
    {
        if(transform.eulerAngles.y == 180)
        {
            direction = -1;
        }
        else if(transform.eulerAngles.y == 0)
        {
            direction = 1;
        }
    }
        
}

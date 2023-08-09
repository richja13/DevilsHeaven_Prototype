using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class PlayerHp : MonoBehaviour
{
    public float maxHp = 8;
    public float currentHp;
    public Image[] Hp;
    int h;
    float knockbackForce;
    public int direction;
    public GameObject HealParticles;
    public RectTransform HealPotion;
    private Vector2[] HealPotionStates;
    private float MaxHeal = 10;
    public float CurrentHeal;

    public static PlayerHp Instance {get; private set;}    

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentHp = maxHp;
        CurrentHeal = MaxHeal;
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckDirection();
        HealPotionPosition();
        Debug.Log((CurrentHeal - MaxHeal)/ 10);

        if (Input.GetMouseButtonDown(2))
        {

            HealAnimation();

            PlayerController.Instance.CanMove = false;

        }

        if (currentHp <= 0)
        {
           // Destroy(this.gameObject);
            //DeathAnimation method
        }

        HpBar();
    }

    bool Death()
    {
        if (currentHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GetDamage(float Dmg)
    {
        currentHp -= Dmg;
     //   CameraShaker.Instance.ShakeOnce(2f, 1f, 0.2f, 0.6f);
          CinemaMachineShake.Instance.ShakeCamera(4,.4f);
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(300 * direction, 500) * knockbackForce * Time.fixedDeltaTime;
        PlayerController.Instance.GetHit();
    }

    void HpBar()
    {
      

        h = Mathf.FloorToInt(currentHp);
        Hp[h].GetComponent<Animator>().SetBool("NoHp", true);

      
    }

    void CheckDirection()
    {
        if (transform.eulerAngles.y == 180)
        {
            direction = -1;
        }
        else if (transform.eulerAngles.y == 0)
        {
            direction = 1;
        }
    }

    public void HealAnimation()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Heal", true);
    }

    public void EndHealAnimation()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Heal", false);
    }

    void Heal()
    {
        //anim.SetTrigger();
        if (currentHp <= maxHp)
        {
            currentHp = currentHp + 1f;

            if (Hp[h].GetComponent<Animator>().GetBool("NoHp") == true)
            {
                Hp[h].GetComponent<Animator>().SetBool("NoHp", false);
            }

            Instantiate(HealParticles, new Vector2(transform.position.x + 0.1f, transform.position.y - 0.2f), transform.rotation);
           
        }

        MinusHealPotionPosition();
    }


    void HealPotionPosition()
    {

        Debug.Log(CurrentHeal + " - Current Heal");
        if (CurrentHeal > -6)
        {
            HealPotion.anchoredPosition = new Vector2(HealPotion.anchoredPosition.x, (CurrentHeal - MaxHeal));
            
        }
    }

    void MinusHealPotionPosition()
    {
        CurrentHeal -= 3;
    }

    public void AddHealPotion(float Heal)
    {
        if (!HealPotionFull())
        {
            CurrentHeal += Heal;
        }
    }

    bool HealPotionFull()
    {
        if(HealPotion.anchoredPosition.y >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

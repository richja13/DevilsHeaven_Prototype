using System.Threading;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.eulerAngles.y == 180)
        {
            transform.position += Vector3.right * 20f * Time.fixedDeltaTime;
        }
        else
        {
            transform.position += Vector3.right * -20f * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHp.Instance.GetDamage(Damage);
            PlayerController.Instance.PlayBlood();
            Destroy(this.gameObject);
        }
    }
}

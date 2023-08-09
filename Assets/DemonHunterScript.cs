
using UnityEngine;

public class DemonHunterScript : MonoBehaviour
{
    public Transform ArrowPos;
    public GameObject Arrow;
    public LayerMask GroundLayer;
    public LayerMask PlayerLayer;
    UnityEngine.Vector2 RaycastVector;
    private Animator anim;
    public Animator ArrowAnim;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerDetected();

        if(PlayerDetected())
        {
            anim.SetTrigger("Shoot");
          
        }
    }

    void ChargeArrow()
    {
          ArrowAnim.SetTrigger("ArrowCharge");
    }

    void ShootArrow()
    {
        Instantiate(Arrow, ArrowPos.position, transform.rotation);
       rb.velocity  = -transform.right * Time.fixedDeltaTime * 80;
    }

    public bool PlayerDetected()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float distance = 12f;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.right, distance,PlayerLayer);
        UnityEngine.Debug.DrawRay(position, RaycastVector, Color.red,distance);

        if (hit.collider == null)
        {
             //Debug.Log("NIE WYKRYTY");
        }
        else
        {
            //Debug.Log(hit.collider.name);
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
}

using UnityEngine;

public class parryCheck : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    private PlayerMove playermove;
    private CircleCollider2D circleCollider;


    private void Awake()
    {
        rbody2D = GetComponentInParent<Rigidbody2D>();
        playermove = GetComponentInParent<PlayerMove>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ParryBullet") 
        {
            playermove.Jump();

            circleCollider.enabled = false;
            playermove.JumpControl(false);
        }
    }
}

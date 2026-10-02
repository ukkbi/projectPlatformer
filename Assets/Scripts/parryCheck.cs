using UnityEngine;

public class parryCheck : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    private PlayerMove playermove;


    private void Awake()
    {
        rbody2D = GetComponentInParent<Rigidbody2D>();
        playermove = GetComponentInParent<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.gameObject.tag == "ParryBullet") 
        {

            rbody2D.AddForce(Vector2.up * playermove.jumpPower, ForceMode2D.Impulse);
            playermove.JumpControl(false);
        }
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 플레이어 이동 - 이동, 점프
/// </summary>
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid2D;
    private CircleCollider2D circleCollider;

    [SerializeField]
    private float speed = 2.0f;

    [SerializeField]
    public float jumpPower = 2f;
    private int count = 0;
    private int maxJumpCount = 2;

    private bool bJump = true;
    public void JumpControl(bool bcontrol) 
    { 
      bJump = bcontrol;  
    }

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float dir = Input.GetAxis("Horizontal");
        Move(dir);


        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            count++;

            if (count < maxJumpCount && bJump == true)
                Jump();

            Debug.Log($"count : {count}");
            if (count >= 2)
            {
                StartCoroutine(Co_parring());
            }
        }
            
    }


    /// <summary>
    /// 이동
    /// </summary>
    /// <param name="dir">방향</param>
    public void Move(float dir)
    {
        rigid2D.linearVelocityX = speed * dir;
    }

    /// <summary>
    /// 점프
    /// </summary>
    public void Jump()
    {
       rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            ClearCount();
    }


    private IEnumerator Co_parring() 
    {
        circleCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        circleCollider.enabled = false;
    }
    public void ClearCount() 
    {
        count = 0;
    }
}

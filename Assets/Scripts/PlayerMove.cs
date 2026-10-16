using System;
using System.Collections;
using UnityEngine;



/// <summary>
/// 플레이어 이동 - 이동, 점프
/// </summary>
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid2D;
    private CircleCollider2D circleCollider;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject firePos;
    private float firePosSpace = 0.8f;

    [SerializeField]
    public float jumpPower = 2f;
    [SerializeField]
    public float speed = 2.0f;
    
    private int count = 0;
    private int maxJumpCount = 2;

    private int offsetCount = Enum.GetValues(typeof(Offset)).Length;

    private float facing = 1f;

    private Vector2 sclae;
    public Vector3[] aimDirection;

    private Vector3 fireDirection = new Vector3(1,0,0);
    public Vector3 FireDirection => fireDirection;
    
    private bool bJump = true;

    private enum Offset 
    { 
        right,
        upRight,
        up
    }

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();

        circleCollider.enabled = false;

        sclae = transform.localScale;


    }

    private void Start()
    {
        aimDirection = new Vector3[offsetCount];
    }

    // Update is called once per frame
    void Update()
    {
        float dirHorizontal = Input.GetAxis("Horizontal");
        float dirVertical = Input.GetAxis("Vertical");

        if(dirHorizontal != 0)facing = dirHorizontal > 0 ? 1: -1;
        fireDirection.x *= facing;
        spriteRenderer.transform.localScale = new Vector3(facing, 1, 0);
        if (Input.GetKey(KeyCode.X))
        {
            fireDirection = AimOffset(VectorToAim(dirVertical, dirHorizontal));
            fireDirection.x *= facing;

            firePos.transform.localPosition = new Vector3(0.8f, 0, 0);
            firePos.transform.localPosition = new Vector3(fireDirection.x * firePosSpace * facing,fireDirection.y*Mathf.Abs(firePosSpace));
        }
        else 
        { 
            Move(dirHorizontal);

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
        bJump = false;
        rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            ResetBjump();
    }


    private IEnumerator Co_parring() 
    {   
        Vector2 changeSclae = sclae;
        changeSclae.y = 1f;
        transform.localScale = changeSclae;

        circleCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        transform.localScale = sclae;
        circleCollider.enabled = false;

    }

    /// <summary>
    ///  점프 횟수 초기화 및 점프 bool 값 수정
    /// </summary>
    public void ResetBjump() 
    {
        count = 0;
        bJump = true;
    }

    private Offset VectorToAim(float v, float h) 
    {
        bool isForward = h != 0;
        if (v > 0)
            return isForward ? Offset.upRight : Offset.up;
        else return Offset.right;
    }

    private Vector3 AimOffset(Offset offset) 
    {
        switch (offset) 
        {
            case Offset.upRight:
                return new Vector3(1,1,0);
            case Offset.up:
                return Vector3.up;
            default:
                return Vector3.right;
                
        }
    }
}

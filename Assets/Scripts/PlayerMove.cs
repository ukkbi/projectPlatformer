using System;
using System.Collections;
using UnityEngine;


// 상태
// 일반, 고정 사격, 패링

// 상태 종류를 enum으로 정의
// 상태 값을 Player를 대표하는 클래스가 갖고 멤버 변수로 갖고 있는다.

// 멤버 변수들이 많아지면 같은 역할끼리 주석이나 헤더  ([Header("인스펙터뷰에 표시할 내용")])


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
    public float Facing => facing;

    private Vector2 sclae;

    private Vector3 fireDirection = new Vector3(1,0,0);
    public Vector3 FireDirection => fireDirection;
    
    private bool bJump = true;
    private bool bDash = false; // 대쉬  체크용
    private float dashStartTime; // 대쉬 시작 시간
    private float gravityValue; // 공중 대쉬 시를 위한 중력값 저장용
    [SerializeField]
    private float dashSpeed = 5f; // 대쉬 속도
    private float dashCooldown = 2f; // 쿨타임
    private float dashCooling = 0; // 쿨 다운 중인지 확이용



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
        dashCooling = dashCooldown * -1f;
    }

    // Update is called once per frame
    void Update()
    {
        float dirHorizontal = Input.GetAxis("Horizontal");
        float dirVertical = Input.GetAxis("Vertical");

        
        if (Input.GetKeyDown(KeyCode.LeftShift) && bDash == false)
        {
            if ((Time.time - dashCooling) < dashCooldown)
                return;

            dashStartTime = Time.time;
            gravityValue = rigid2D.gravityScale;
            bDash = true;
        }


        if (dirHorizontal != 0) facing = dirHorizontal > 0 ? 1 : -1;

        spriteRenderer.transform.localScale = new Vector3(facing, 1, 0);
        if (Input.GetKey(KeyCode.X))
        {
            fireDirection = AimOffset(VectorToAim(dirVertical, dirHorizontal));
            spriteRenderer.transform.localScale = new Vector3(facing, 1, 0);
            firePos.transform.localPosition = new Vector3(fireDirection.x * firePosSpace, fireDirection.y * Mathf.Abs(firePosSpace));
        }
        else 
        {
            firePos.transform.localPosition = new Vector3(0.8f, 0, 0);

            Move(dirHorizontal);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                count++;

                if (count < maxJumpCount && bJump == true)
                    Jump(jumpPower);

                Debug.Log($"count : {count}");
                if (count >= 2)
                {
                    StartCoroutine(Co_parring());
                }
            }

        }//else
    }

    private void FixedUpdate()
    {
        
        //대쉬
        if (bDash == true) 
        {
            if ((Time.time - dashStartTime) < 0.3f)
            {
                rigid2D.gravityScale = 0;
                rigid2D.linearVelocityY = 0;
                rigid2D.linearVelocityX = dashSpeed * facing;
                
            }
            else 
            {
                bDash = false;
                rigid2D.gravityScale = gravityValue;
                dashCooling = Time.time;
            }
            
            
        }//대쉬  if문


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
    public void Jump(float jumpPower)
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

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
public class Player : MonoBehaviour
{
    // 플레이어 상태
    private enum State
    {
        move,
        fixedFire,
        parry,
        dash
    }
    private State state;


    private enum Offset
    {
        right,
        upRight,
        up
    }

    private Rigidbody2D rigid2D;
    private CircleCollider2D circleCollider;

    // 캐릭터 랜더링용
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("이동속도")]
    [SerializeField]
    public float speed = 2.0f;

    [Header("점프")]
    [SerializeField]
    public float jumpPower = 2f;
    private int count = 0;
    private int maxJumpCount = 2;
    private bool bJump = true;

    //플레이어가 보고 있는 방향
    private float facing = 1f;
    public float Facing => facing;

    private Vector2 sclae;

    [Header("사격")]
    [SerializeField] private GameObject firePos; // 발사지점
    [SerializeField] private float bulletDamage = 5f;
    public float BulletDamage => bulletDamage;
    private float firePosSpace = 0.8f; // 발사 지점의 기본 거리
    private Vector3 fireDirection = new Vector3(1, 0, 0); // 발사 방향
    public Vector3 FireDirection => fireDirection;

    [Header("대쉬")]
    private bool bDash = false; // 대쉬  체크용
    private float dashTime; // 대쉬 종료됐는지 확인용
    [SerializeField] private float dashDuration = 0.3f; // 대쉬 시간
    private float gravityValue; // 공중 대쉬 시를 위한 중력값 저장용
    [SerializeField] private float dashSpeed = 5f; // 대쉬 속도
    private float dashCooldown = 2f; // 쿨타임
    private float dashCoolTime = 0; // 쿨 다운 중인지 확인용



    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();

        circleCollider.enabled = false;

        sclae = transform.localScale;


    }

    private void Start()
    {
        dashCoolTime = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        float dirHorizontal = Input.GetAxis("Horizontal");
        float dirVertical = Input.GetAxis("Vertical");

        CoolTimeCalc();


        //if (dirHorizontal != 0) facing = dirHorizontal > 0 ? 1 : -1;

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

            switch (state)
            {
                case State.move:
                    HandleMove(dirHorizontal);
                    HandleDash();
                    break;
                case State.parry:
                    break;
                case State.fixedFire:
                    break;
                case State.dash:
                    Dashing();
                    break;
            }

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

    }

    private void HandleMove(float dir) 
    { 
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            Move(dir);
        }
    }

    private void HandleDash()
    {
        
    }

    private void CoolTimeCalc() 
    {
        if (dashCoolTime < dashCooldown)
            dashCoolTime += Time.deltaTime;
    }

    /// <summary>
    /// 이동
    /// </summary>
    /// <param name="dir">방향</param>
    public void Move(float dir)
    {
        Debug.Log("실행됨");
        rigid2D.linearVelocityX = speed * dir;
        if(dir > 0)
            facing = 1;
        else if(dir < 0)
            facing = -1;
    }

    /// <summary>
    /// 점프
    /// </summary>
    public void Jump(float jumpPower)
    {
        bJump = false;
        rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

    }

    public void Dash()
    {
        if (dashCoolTime < dashCooldown)
        {
            Debug.Log($"coolTime : {dashCoolTime}");
            return;
        }

        dashCoolTime = 0;
        state = State.dash;

        rigid2D.gravityScale = 0;
        rigid2D.linearVelocityY = 0;
        rigid2D.linearVelocityX = dashSpeed * facing;
        Debug.Log($" 속도 : {rigid2D.linearVelocityX}");

        dashTime = 0;
    }

    private void Dashing() 
    {
        dashTime += Time.deltaTime;
        if (dashTime > dashDuration)
            End_Dash();
    }

    private void End_Dash() 
    {
        rigid2D.gravityScale = gravityValue;

        state = State.move;
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

    #region 발사 방향 결정 함수
    /// <summary>
    /// 입력값에 따른 발사 방향 정보 결정
    /// </summary>
    /// <param name="v"></param>
    /// <param name="h"></param>
    /// <returns></returns>
    private Offset VectorToAim(float v, float h)
    {
        bool isForward = h != 0;
        if (v > 0)
            return isForward ? Offset.upRight : Offset.up;
        else return Offset.right;
    }
    
    /// <summary>
    /// 방향 정보로 발사 방향 결정
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 AimOffset(Offset offset)
    {
        switch (offset)
        {
            case Offset.upRight:
                return new Vector3(1, 1, 0);
            case Offset.up:
                return Vector3.up;
            default:
                return Vector3.right;

        }
    }
    #endregion
}

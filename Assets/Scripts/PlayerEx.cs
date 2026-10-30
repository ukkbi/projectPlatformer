using UnityEngine;

public class PlayerEx : MonoBehaviour
{
    /* 상태정의 */
    public enum State
    {
        move,
        fixedFire,
        parry,
        dash
    }
    [SerializeField]
    private State state;
    /* 상태정의 */


    [SerializeField]
    private Rigidbody2D rigid2D;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // ----- 입력 ----- //
    float vInput;
    float hInput;
    // ----- 입력 ----- //

    // ----- 이동 ----- //
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private int direction = 1;



    Vector3 rendererScale;
    // ----- 이동 ----- //


    private void Awake()
    {
        rendererScale = spriteRenderer.transform.localScale;
    }


    private void Update()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");

        switch (state)
        {
            case State.move:
                HandleMove();
                HandleDash();
                break;
            case State.dash:
                UpdateDash();
                break;
        }
    }


    void HandleMove()
    {
        if(hInput > 0.1f)
        {
            Move(1);
        }
        else if(hInput < -0.1f)
        {
            Move(-1);
        }
        else
        {
            Move(0);
        }
    }

    void Move(int dir)
    {
        if(dir == 0)
        {
            rigid2D.linearVelocityX = 0;
            return;
        }

        // 방향값 설정
        direction = dir;

        // 리지드바디 x 속력 설정
        rigid2D.linearVelocityX = direction * moveSpeed;

        // 렌더러 방향
        spriteRenderer.transform.localScale = new Vector3(direction * rendererScale.x, rendererScale.y, rendererScale.z);
    }



    void HandleDash()
    {
        // 대시 키 눌렸을 때 BeginDash() 실행
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            gravityValue = rigid2D.gravityScale;
            Dash();
        }
    }


    /// <summary>
    /// 대시 시작
    /// </summary>
    void BeginDash()
    {
        // 대시 가능한 상황인지 체크

        // 대시 상태로 전환

        // 대시 동작을 위해 필요한 것들 설정
    }

    /// <summary>
    /// 대시 중
    /// </summary>
    void UpdateDash()
    {
        // 타이머, 지속시간 비교

        // 타이머가 지속시간보다 커지면 대시 종료
    }

    /// <summary>
    /// 대시 종료
    /// </summary>
    void EndDash()
    {
        // 일반 상태로 전환

        // 대시 동작을 위해 필요한 것들 설정들을 본래 상태로 되돌리기
    }
}

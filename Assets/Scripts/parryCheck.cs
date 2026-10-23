using System;
using UnityEngine;

public class ParryCheck : MonoBehaviour
{
    private PlayerMove playermove;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rigidBody2D;

    private float parryJumpValue = 4f; //패링 후에 튀어오르는 값

    public event Action OnParry;

    private void Awake()
    {
        //플레이어 본체
        playermove = GetComponentInParent<PlayerMove>();
        rigidBody2D = GetComponentInParent<Rigidbody2D>();

        //패링 판정용
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IParryable parryable))
        {
            rigidBody2D.linearVelocityY = 0;

            playermove.Jump(parryJumpValue);

            circleCollider.enabled = false;
            Debug.Log("패링 실행됨");
            parryable.OnParried();

        }
            
    }
}

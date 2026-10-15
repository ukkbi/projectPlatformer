using System;
using UnityEngine;

public class ParryCheck : MonoBehaviour
{
    private PlayerMove playermove;
    private CircleCollider2D circleCollider;

    public event Action OnParry;

    private void Awake()
    {
        playermove = GetComponentInParent<PlayerMove>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IParryable parryable))
        {
            playermove.Jump();

            circleCollider.enabled = false;
            Debug.Log("ÆÐ¸µ ½ÇÇàµÊ");
            parryable.OnParried();

        }
            
    }
}

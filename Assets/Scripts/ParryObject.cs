using System.Collections;
using UnityEngine;

public class ParryObject : MonoBehaviour, IParryable
{
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnParried()
    {
        StartCoroutine(Co_CoolDown());

    }

    private IEnumerator Co_CoolDown() 
    {
        spriteRenderer.color = Color.gray;
        circleCollider.enabled = false;
        yield return new WaitForSeconds(1.7f);

        spriteRenderer.color = Color.white;
        circleCollider.enabled = true;
    }

}

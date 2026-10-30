using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float maxHp = 100f;
    private float curHp = 0f;


    private void Start()
    {
        curHp = maxHp;
    }

    private void Update()
    {
        if(curHp <= 0)
            Destroy(gameObject);
    }

    public void TakeDamage(float damage) 
    {
        curHp -= damage;
        curHp = Mathf.Clamp(curHp, 0, maxHp);

        Debug.Log($"{curHp}");
    }
}

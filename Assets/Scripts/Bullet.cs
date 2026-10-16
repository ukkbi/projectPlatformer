using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    
    [SerializeField]
    private float speed = 5f;

    public float moveSpeed {get; } = 2f;

    public Vector3 direction;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }
    // Update is called once per frame
    void Update()
    {
        float dir = direction.x;
        
        if (Mathf.Abs(dir) > 0)
            rigidbody2D.linearVelocity = direction * speed;
        else
        {
            Debug.Log($"현재 방향 값{direction}");
            rigidbody2D.linearVelocity = direction * speed;
        }
    }
}

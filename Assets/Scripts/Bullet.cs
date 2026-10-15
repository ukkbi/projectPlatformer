using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    public float dir { get; set; }
    public float moveSpeed { get; set; } = 2f;
    private int switchInt = 0;

    public Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Mathf.Abs(dir) > 0)
            transform.Translate(direction * speed * moveSpeed * Time.deltaTime, Space.World);
        else
        {
            Debug.Log($"현재 방향 값{direction}");
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
            



    }
}

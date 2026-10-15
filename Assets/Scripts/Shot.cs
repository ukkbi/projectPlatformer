using System.Collections;
using System.Threading;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private PlayerMove player;

    [SerializeField]
    private Transform firePos;
    [SerializeField]
    private GameObject BulletPrefab;

    [SerializeField]
    private float shotCoolDown = 0.3f;
    
    private float cooltime = -1;

    private int i = 0;

    private void Awake()
    {
        player = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {


            Fire();
            
        }
            
    }
    public void Fire() 
    {
        float dir = Input.GetAxis("Horizontal");

        if (Time.time - cooltime <= shotCoolDown)
            return;

            i++;
        Vector3 pos;
        Vector3 pos1 = firePos.position + new Vector3(0, 0.3f, 0);
        Vector3 pos2 = firePos.position + new Vector3(0, -0.3f, 0);

        int calc = i % 4;
        switch (calc)
        {
            case 1:
                Debug.Log($"½ÇÇàµÊ{calc}");
                pos = pos1;
                break;
            case 2:
                Debug.Log($"½ÇÇàµÊ{calc}");
                pos = firePos.position;
                break;
            case 3:
                Debug.Log($"½ÇÇàµÊ{calc}");
                pos = pos2;
                break;
            case 0:
                Debug.Log($"½ÇÇàµÊ{calc}");
                pos = firePos.position;
                break;
            default:
                pos = firePos.position;
                break;
        }

        GameObject bullet = Instantiate(BulletPrefab, pos, Quaternion.Euler(0, 0, 90));
        Bullet bulletComponent =  bullet.GetComponent<Bullet>();
        bulletComponent.dir = dir;
        bulletComponent.direction = player.direction;

        cooltime = Time.time;

        //GameObject bullet = Instantiate<GameObject>(BulletPrefab, firePos.position, Quaternion.identity);

        //bullet.transform.Translate(Vector3.right * 3);

    }
}

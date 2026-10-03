using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]
    private Transform firePos;
    [SerializeField]
    private GameObject BulletPrefab;

    private void Fire() 
    {
        GameObject bullet = Instantiate<GameObject>(BulletPrefab, firePos.position, Quaternion.identity);



    }
}

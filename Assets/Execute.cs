using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execute : MonoBehaviour
{
    public float speed = 2f;
    //[SerializeField] private Projectile prefab;
    [SerializeField] private PoolPreparer poolPreparer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //var projectile = prefab.Get<Projectile>(transform,Vector3.up,Quaternion.identity);
            var projectile = poolPreparer.prefabs[0].Get<Projectile>(transform);
            projectile.transform.position = transform.position;
            projectile.GetComponent<Rigidbody2D>().velocity = Vector3.up;
        }
    }
}

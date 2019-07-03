using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledMonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shredder"))
        {
            gameObject.SetActive(false);
        }
    }
}

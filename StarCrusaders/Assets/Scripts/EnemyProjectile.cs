using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float screenLimit = 15f;

    void Update()
    {
        DestroyOffscreen();
    }

    void DestroyOffscreen()
    {
        if (transform.position.x < -screenLimit || transform.position.x > screenLimit)
            Destroy(gameObject);
        if (transform.position.y < -screenLimit || transform.position.y > screenLimit)
            Destroy(gameObject);
    }
}

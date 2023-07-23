using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] GameObject impactParticle;
    float screenLimit = 15;

    void Update()
    {
        CalculateMovement();
        DestroyOffscreen();
    }

    void CalculateMovement()
    {
        Vector2 direction = Vector2.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void DestroyOffscreen()
    {
        if (transform.position.x > screenLimit)
            Destroy(gameObject);
    }

    void DoDamage()
    {
        // Setup for when trigger collision impact life of other
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        DoDamage();
        Instantiate(impactParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

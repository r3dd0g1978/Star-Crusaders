using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    float screenLimit = 8f;

    void Update()
    {
        DestroyOffScreen();
    }

    void DestroyOffScreen()
    {
        if (transform.position.x <= -screenLimit || transform.position.x >= screenLimit || transform.position.y <= -screenLimit || transform.position.y >= screenLimit)
            Destroy(gameObject);
    }
}

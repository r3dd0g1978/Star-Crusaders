using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float triggerDistanceFromPlayer = 0.5f;
    //string[] powerupTypeNames = { "bomb", "shield", "speed", "laser", "multi" };
    string[] powerupTypeNames = { "shield", "speed" };


    Player player;
    PowerupsManager powerupsManager;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        powerupsManager = FindObjectOfType<PowerupsManager>();
    }

    void Update()
    {
        CalculateMovement();
        TriggerPowerUp();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    void TriggerPowerUp()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= triggerDistanceFromPlayer)
            {
                int randomPowerUpIndex = Random.Range(0, powerupTypeNames.Length);
                string randomPowerUpName = powerupTypeNames[randomPowerUpIndex];
                powerupsManager.StartPowerupSequence(randomPowerUpName);
                Destroy(gameObject);
            }
        }
    }
}

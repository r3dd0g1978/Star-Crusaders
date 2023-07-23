using System.Collections;
using UnityEngine;

public class BossOne : MonoBehaviour
{
    public delegate void BossDied();
    public static event BossDied OnBossDied;

    enum MovementState { Launching, Roaming, }
    MovementState currentState;

    enum WeaponState { Basic, Homing, }
    [Header("Weapon Properties")]
    [SerializeField] WeaponState currentWeaponState;
    [SerializeField] float projectileSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float coolDown;
    [SerializeField] float projectileLimit;
    [Header("Other Properties")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float screenLimit = 1f;
    [SerializeField] float triggerStatePos = 2f;
    [SerializeField] float health = 5f;

    Vector2 movementDirection;
    EnemyArsenal enemyArsenal;
    bool routineStarted = false;

    void Awake()
    {
        enemyArsenal = FindObjectOfType<EnemyArsenal>();
    }

    void Start()
    {
        currentState = MovementState.Launching;
        enemyArsenal.Shoot(currentWeaponState.ToString(), this.gameObject, projectileSpeed, fireRate, coolDown, projectileLimit);
    }

    void Update()
    {
        LaunchMovement();
        RoamingMovement();
    }

    void LaunchMovement()
    {
        if (currentState == MovementState.Launching)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            if (transform.position.x <= triggerStatePos)
            {
                movementDirection = new Vector2(transform.position.x, transform.position.y);
                currentState = MovementState.Roaming;
            }
        }
    }

    void RoamingMovement()
    {
        if (currentState == MovementState.Roaming)
        {
            if (!routineStarted)
            {
                StartCoroutine(RoamingDirectionRoutine());
                routineStarted = true;
            }

            transform.Translate(movementDirection.normalized * moveSpeed * Time.deltaTime);
            RestrictMovement();
        }
    }

    IEnumerator RoamingDirectionRoutine()
    {
        while (currentState == MovementState.Roaming)
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            movementDirection = new Vector2(randomX, randomY);
            yield return new WaitForSeconds(1f);
        }
    }

    void RestrictMovement()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -screenLimit, screenLimit), (Mathf.Clamp(transform.position.y, -screenLimit, screenLimit)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        if (health == 0)
            Die();
    }

    void Die()
    {
        OnBossDied?.Invoke();
        Destroy(gameObject);
    }
}
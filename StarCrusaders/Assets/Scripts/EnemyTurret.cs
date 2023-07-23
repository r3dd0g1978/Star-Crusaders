using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public delegate void EnemyDied();
    public static event EnemyDied OnEnemyDied;

    enum WeaponState { Basic, Homing, }
    [Header("Weapon Properties")]
    [SerializeField] WeaponState currentWeaponState;
    [SerializeField] float projectileSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float coolDown;
    [SerializeField] float projectileLimit;
    [Header("Other Properties")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float health = 3f;

    EnemyArsenal enemyArsenal;
    Rigidbody2D myrb;

    float screenLimit = 8f;

    void OnEnable()
    {
        BossOne.OnBossDied += Die;
    }

    void OnDisable()
    {
        BossOne.OnBossDied -= Die;
    }

    void Awake()
    {
        enemyArsenal = FindObjectOfType<EnemyArsenal>();
        myrb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.position = new Vector2(transform.position.x, -4.8f);
        enemyArsenal.Shoot(currentWeaponState.ToString(), this.gameObject, projectileSpeed, fireRate, coolDown, projectileLimit);
    }

    void FixedUpdate()
    {
        CalculateMovement();
    }

    void Update()
    {
        DestroyOffScreen();
    }

    void CalculateMovement()
    {
        Vector2 direction = Vector2.left;
        myrb.MovePosition((Vector2)transform.position + (direction * Time.deltaTime * moveSpeed));
    }

    void Die()
    {
        OnEnemyDied?.Invoke();
        Destroy(gameObject);
    }

    void DestroyOffScreen()
    {
        if (transform.position.x < -screenLimit)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        if (health == 0)
            Die();
    }
}

using UnityEngine;

public class EnemyBasic : MonoBehaviour
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
    [SerializeField] GameObject deathParticles;

    EnemyArsenal enemyArsenal;
    Player player;
    SpriteRenderer mySpriteRenderer;
    Rigidbody2D myrb;

    float screenLimit = 10f;
    Vector2 startingPos;

    void Awake()
    {
        enemyArsenal = FindObjectOfType<EnemyArsenal>();
        player = FindObjectOfType<Player>();
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myrb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startingPos = transform.position;
        enemyArsenal.Shoot(currentWeaponState.ToString(), this.gameObject, projectileSpeed, fireRate, coolDown, projectileLimit);
    }

    void FixedUpdate()
    {
        CalculateMovement();
    }

    void Update()
    {
        ResetPosition();
    }

    void CalculateMovement()
    {
        Vector2 direction = (Vector2)transform.position + (Vector2.left * Time.deltaTime * moveSpeed);
        myrb.MovePosition(direction);
    }

    void ResetPosition()
    {
        if (transform.position.x < -screenLimit)
            transform.position = startingPos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Die();
    }

    void Die()
    {
        OnEnemyDied?.Invoke();
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

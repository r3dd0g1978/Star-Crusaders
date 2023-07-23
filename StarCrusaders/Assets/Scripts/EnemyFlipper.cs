using UnityEngine;

public class EnemyFlipper : MonoBehaviour
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
    [SerializeField] float returnSpeed = 10f;
    [SerializeField] GameObject deathParticles;
    [SerializeField] Transform flipPosition;
    [SerializeField] Transform triggerPosition;

    Player player;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    EnemyArsenal enemyArsenal;

    bool isReturning = false;
    bool seekingPlayer = false;

    float screenLimit = 10f;

    const string ANIM_FLIP = "Enemy_Flipper_Flip";

    void Awake()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        enemyArsenal = FindObjectOfType<EnemyArsenal>();
    }

    void Start()
    {
        enemyArsenal.Shoot(currentWeaponState.ToString(), this.gameObject, projectileSpeed, fireRate, coolDown, projectileLimit);
    }

    void FixedUpdate()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        if (!isReturning && !seekingPlayer)
        {
            Vector2 targetPosition = rb.position + Vector2.left * moveSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition);

            if (transform.position.x <= flipPosition.position.x)
            {
                spriteRenderer.flipX = true;
                PlayFlipAnimation();
                isReturning = true;
            }
        }
        else if (isReturning && !seekingPlayer)
        {
            Vector2 targetPosition = rb.position + Vector2.right * returnSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition);
            if (transform.position.x >= triggerPosition.position.x)
            {
                seekingPlayer = true;
            }
        }

        if (isReturning && seekingPlayer)
        {
            spriteRenderer.flipX = false;
            SeekPlayer();
        }
    }

    private void PlayFlipAnimation()
    {
        animator.Play(ANIM_FLIP);
    }

    private void SeekPlayer()
    {
        if (player != null)
        {
            // Calculate the direction towards the player character
            Vector2 direction = player.transform.position - transform.position;

            // Move the enemy towards the player character using MovePosition and Vector2.MoveTowards
            Vector2 targetPosition = rb.position + direction;
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, returnSpeed * Time.deltaTime));
        }
    }

    void DestroyOffScreen()
    {
        if (transform.position.x < -screenLimit)
            Destroy(gameObject);
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

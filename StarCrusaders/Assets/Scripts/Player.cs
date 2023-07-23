using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsHit
    {
        get { return playerIsHit; }
    }
    public float HitTimer
    {
        get { return hitTimer; }
    }
    
    
    public delegate string PlayerHit(string hitSFX);
    public static event PlayerHit OnPlayerHit;
    public delegate string PlayerShoot(string projectileType);
    public static event PlayerShoot OnPlayerShoot;

    [Header("Speed Properties")]
    [SerializeField] float defaultMoveSpeed = 20f;
    [SerializeField] float speedIncrease = 3f;
    [Header("Screen Limits")]
    [SerializeField] float xMovementLimit = 6f;
    [SerializeField] float yMovementLimit = 4f;
    [Header("Misc")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserXOffset;
    [SerializeField] float hitTimer = 3f;

    Animator myAnimator;
    Collider2D myCollider;
    ParticleSystem myParticleSystem;
    PowerupsManager powerupsManager;

    float currentMoveSpeed;
    float yInput = 0;
    float xInput = 0;
    bool playerIsHit = false;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        myParticleSystem = GetComponent<ParticleSystem>();
        powerupsManager = FindObjectOfType<PowerupsManager>();
    }

    void Start()
    {
        currentMoveSpeed = defaultMoveSpeed;
    }

    void Update()
    {
        CalculateMovement();
        Shoot();
    }

    void CalculateMovement()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        Vector2 playerInput = new Vector2(xInput, yInput);
        transform.Translate(playerInput * currentMoveSpeed * Time.deltaTime);

        transform.position = new Vector2((Mathf.Clamp(transform.position.x, -xMovementLimit, xMovementLimit)),
            (Mathf.Clamp(transform.position.y, -yMovementLimit, yMovementLimit)));
        
        if (!playerIsHit)
            MovementAnimation();
    }

    void MovementAnimation()
    {
        if (yInput > 0.1)
            myAnimator.SetTrigger("movingUp");
        else if (yInput < -0.1)
            myAnimator.SetTrigger("movingDown");
        else if (!playerIsHit)
            myAnimator.SetTrigger("base");
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 laserOffset = new Vector2(transform.position.x + laserXOffset, transform.position.y);
            GameObject laserInstance = Instantiate(laserPrefab, laserOffset, Quaternion.identity);
            OnPlayerShoot?.Invoke("PlayerLaserBasic");
        }
    }

    void TakeHit()
    {
        OnPlayerHit?.Invoke("PlayerHit");
        myParticleSystem.Play();
        StartCoroutine(EnterHitState());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 8) //8 is the layer int/index number. Powerups on this layer. Improve?
        {
            Destroy(other.gameObject);
            TakeHit();    
        }
    }

    IEnumerator EnterHitState()
    {
        playerIsHit = true;
        while (playerIsHit)
        {
            myCollider.enabled = false;
            myAnimator.SetBool("playerHurt", playerIsHit);
            yield return new WaitForSeconds(hitTimer);
            playerIsHit = false;
        }
        myCollider.enabled = true;
        myAnimator.SetBool("playerHurt", playerIsHit);
    }

    public IEnumerator SpeedPowerUp()
    {
        while (true)
        {
            currentMoveSpeed += speedIncrease;
            yield return new WaitForSeconds(powerupsManager.SpeedBoostTimer);
            currentMoveSpeed = defaultMoveSpeed;
            break;
        }
    }

    public void TogglePlayerCollider()
    {
        if (myCollider.isActiveAndEnabled == true)
            myCollider.enabled = false;
        else
            myCollider.enabled = true;
    }

    public float GetPlayerHitTimerLength()
    {
        return hitTimer;
    }
}

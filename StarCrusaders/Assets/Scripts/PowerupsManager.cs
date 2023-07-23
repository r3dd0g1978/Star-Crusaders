using UnityEngine;

public class PowerupsManager : MonoBehaviour
{
    
    public float SpeedBoostTimer { get => speedboostTimer; }

    [Header("Trigger Values")]
    [SerializeField] int powerupTriggerAmount = 10;
    [SerializeField] int powerupTriggerIncrement = 10;
    int powerupCount;

    [Header("Timers")]
    [SerializeField] float speedboostTimer = 5f;

    [Header("Visuals")]
    [SerializeField] GameObject[] powerupPrefabs;
    [SerializeField] GameObject[] powerupVisuals;
    [SerializeField] Transform[] powerupSpawnPoints;

    Player player;

    void OnEnable()
    {
        EnemyBasic.OnEnemyDied += CalculatePowerupTrigger;
        Crab.OnEnemyDied += CalculatePowerupTrigger;
    }

    void OnDisable()
    {
        EnemyBasic.OnEnemyDied -= CalculatePowerupTrigger;
        Crab.OnEnemyDied -= CalculatePowerupTrigger;
    }

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        powerupCount = 0;
    }

    void CalculatePowerupTrigger()
    {
        powerupCount++;

        if (powerupCount >= powerupTriggerAmount)
        {
            InstantiatePowerup();
            powerupTriggerAmount += powerupTriggerIncrement;
            powerupCount = 0;
        }
    }

    int GetRandomSpawnPosition()
    {
        int randomArrayPos = Random.Range(0, powerupSpawnPoints.Length);
        return randomArrayPos;
    }

    void InstantiatePowerup()
    {
        int randomSpawnPos = GetRandomSpawnPosition();
        GameObject powerup = Instantiate(powerupPrefabs[0], powerupSpawnPoints[randomSpawnPos].transform.position, Quaternion.identity);
    }

    public void StartPowerupSequence(string powerupTypeName)
    {
        switch (powerupTypeName)
        {
            case "bomb":
                Debug.Log("BOMB POWERUP");
                break;
            case "shield":
                Debug.Log("SHIELD POWERUP");
                DisplayPowerupVisuals("shield", 100f);
                break;
            case "speed":
                Debug.Log("SPEED POWERUP");
                StartCoroutine(player.SpeedPowerUp());
                DisplayPowerupVisuals("speed", speedboostTimer);
                break;
            case "laser":
                Debug.Log("LASER POWERUP");
                break;
            case "multi":
                Debug.Log("MULTI");
                break;
            default:
                break;
        }
    }

    public void DisplayPowerupVisuals(string powerupVisualName, float visualTimer)
    {
        switch (powerupVisualName)
        {
            case "shield":
                Instantiate(powerupVisuals[1], player.transform.position, Quaternion.identity);
                break;
            case "speed":
                GameObject speedBoostVisual = Instantiate(powerupVisuals[0], player.transform.position, Quaternion.identity);
                speedBoostVisual.transform.SetParent(player.transform);
                Destroy(speedBoostVisual, visualTimer);
                break;
            default:
                break;
        }
    }

}

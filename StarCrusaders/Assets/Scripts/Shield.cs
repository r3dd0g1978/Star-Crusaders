using UnityEngine;

public class Shield : MonoBehaviour
{

    [SerializeField] int shieldCharges = 9;

    Player player;
    
    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        transform.SetParent(player.transform);
        player.TogglePlayerCollider();
    }

    void Update()
    {
        CalculateCharges();
    }

    void CalculateCharges()
    {
        if (shieldCharges == 0)
        {
            player.TogglePlayerCollider();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        shieldCharges--;
    }
}

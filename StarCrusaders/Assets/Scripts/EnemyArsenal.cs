using System.Collections;
using UnityEngine;

public class EnemyArsenal : MonoBehaviour
{	
	[SerializeField] GameObject basicProjectilePrefab;
	[SerializeField] GameObject homingProjectilePrefab;
    [SerializeField] GameObject bossOneProjectilePrefab;
    
    Player player;

    private void Awake()
    {
		player = FindObjectOfType<Player>();
    }

	public void Shoot(string weapon, GameObject enemy, float projectileSpeed, float fireRate, float coolDown, float projectileLimit)
	{
		switch (weapon)
		{
			case "Basic":
				StartCoroutine(BasicRoutine(enemy, projectileSpeed, fireRate, coolDown, projectileLimit));
                break;
			case "Homing":
				StartCoroutine(HomingRoutine(enemy, projectileSpeed, fireRate, coolDown, projectileLimit));
				break;
			default:
				break;
		}
	}

    IEnumerator BasicRoutine(GameObject enemy, float projectileSpeed, float fireRate, float coolDown, float projectileLimit)
    {
        while (enemy != null)
		{
			for (int i = 0; i < projectileLimit; i++)
			{
                if (player != null && enemy != null)
				{
					GameObject basic = Instantiate(basicProjectilePrefab, enemy.transform.position, Quaternion.identity);
					Rigidbody2D basicRB = basic.GetComponent<Rigidbody2D>();
					basicRB.velocity = Vector2.left * projectileSpeed;
					yield return new WaitForSeconds(fireRate);
				}
			}
            yield return new WaitForSeconds(coolDown);
        }
    }

	IEnumerator HomingRoutine(GameObject enemy, float projectileSpeed, float fireRate, float coolDown, float projectileLimit)
    {
		while (enemy != null)
		{
			for (int i = 0; i < projectileLimit; i++)
			{
				if (player != null && enemy != null)
				{
					GameObject homing = Instantiate(homingProjectilePrefab, enemy.transform.position, Quaternion.identity);
					Rigidbody2D homingRb = homing.GetComponent<Rigidbody2D>();
					Vector2 direction = player.transform.position - homing.transform.position;
					homingRb.velocity = direction.normalized * projectileSpeed;
					yield return new WaitForSeconds(fireRate);
				}
			}
            yield return new WaitForSeconds(coolDown);
		}
	}
}

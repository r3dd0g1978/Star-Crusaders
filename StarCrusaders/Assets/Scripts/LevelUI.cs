using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Image playerHitRedScreenFlash;
    [SerializeField] GameObject playerPortrait;
    [SerializeField] float flashSpeed = 0.02f;

    Player player;
    Animator playerPortraitAnimator;

    float playerHitTimer;

    void OnEnable()
    {
        Player.OnPlayerHit += PlayerHitVisuals;
    }

    void OnDisable()
    {
        Player.OnPlayerHit -= PlayerHitVisuals;
    }

    void Awake()
    {
        player = FindObjectOfType<Player>();
        playerPortraitAnimator = playerPortrait.GetComponent<Animator>();
    }

    string PlayerHitVisuals(string hitSFX)
    {
        StartCoroutine(PlayerHitVisualRoutine());
        return hitSFX;
    }

    IEnumerator PlayerHitVisualRoutine()
    {
        playerHitRedScreenFlash.color = Color.red;
        yield return new WaitForSeconds(flashSpeed);
        playerHitRedScreenFlash.color = Color.clear;

        playerPortraitAnimator.SetBool("playerIsHit", player.IsHit);
        yield return new WaitForSeconds(player.HitTimer);
        playerPortraitAnimator.SetBool("playerIsHit", player.IsHit);
    }
}

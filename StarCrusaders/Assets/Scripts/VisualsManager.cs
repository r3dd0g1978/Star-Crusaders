using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsManager : MonoBehaviour
{
    [SerializeField] Transform starsBackground;
    [SerializeField] Transform starsForeground;
    [SerializeField] float bgScrollSpeed = 0.2f;
    [SerializeField] float fgScrollSpeed = 1f;

    Vector2 bgStartingPosition;
    Vector2 fgStartingPosition;

    private void Start()
    {
        bgStartingPosition = starsBackground.position;
        fgStartingPosition = starsForeground.position;
    }

    private void Update()
    {
        ScrollBackground();
        ScrollForeground();
    }

    void ScrollBackground()
    {
        starsBackground.transform.Translate(Vector3.left * bgScrollSpeed * Time.deltaTime);
        if (starsBackground.transform.position.x < -6.69)
        {
            starsBackground.transform.position = new Vector2(bgStartingPosition.x, bgStartingPosition.y);
        }
    }

    void ScrollForeground()
    {
        starsForeground.transform.Translate(Vector3.left * fgScrollSpeed * Time.deltaTime);
        if (starsForeground.transform.position.x < -7.49)
        {
            starsForeground.transform.position = new Vector2(fgStartingPosition.x, fgStartingPosition.y);
        }
    }
}

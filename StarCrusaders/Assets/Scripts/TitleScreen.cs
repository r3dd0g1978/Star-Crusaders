using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Level_01");
    }
}

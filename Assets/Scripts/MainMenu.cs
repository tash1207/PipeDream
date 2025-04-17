using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
    }

    public void RetryLevel()
    {
        ScoreKeeper.Instance.SetScoreToBeforeLevel();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                LoadLevel1();
                break;
            case 2:
                LoadLevel2();
                break;
            case 3:
                LoadLevel3();
                break;
        }
    }

    public void NextLevel()
    {
        ScoreKeeper.Instance.LockInCurrentScore();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                LoadLevel2();
                break;
            case 2:
                LoadLevel3();
                break;
        }
    }

    public void LoadLevel1()
    {
        GameManager.Instance.LoadLevel1();
    }

    public void LoadLevel2()
    {
        GameManager.Instance.LoadLevel2();
    }

    public void LoadLevel3()
    {
        GameManager.Instance.LoadLevel3();
    }
}

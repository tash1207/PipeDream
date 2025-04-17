using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    ScoreKeeper scoreKeeper;
    public int CurrentLevel { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        }
        else
        {
            Destroy(this);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        scoreKeeper.ResetScore();
        LoadLevel1();
    }

    public void LoadLevel1()
    {
        CurrentLevel = 1;
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2()
    {
        CurrentLevel = 2;
        SceneManager.LoadScene(2);
    }

    public void LoadLevel3()
    {
        CurrentLevel = 3;
        SceneManager.LoadScene(3);
    }
}

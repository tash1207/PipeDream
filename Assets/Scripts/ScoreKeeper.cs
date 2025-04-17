using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance;
    
    int scoreBeforeThisLevel = 0;
    int currentScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public int GetScoreBeforeThisLevel()
    {
        return scoreBeforeThisLevel;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void ModifyScore(int amount)
    {
        currentScore += amount;
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreBeforeThisLevel = 0;
    }

    public void LockInCurrentScore()
    {
        scoreBeforeThisLevel = currentScore;
    }

    public void SetScoreToBeforeLevel()
    {
        currentScore = scoreBeforeThisLevel;
    }
}

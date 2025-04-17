using TMPro;
using UnityEngine;

public class HUDCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text scoreText;

    void Start()
    {
        levelText.text = "Level : " + GameManager.Instance.CurrentLevel;
        UpdateScoreText();
    }

    void Update()
    {
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score : " + ScoreKeeper.Instance.GetScore();
    }
}

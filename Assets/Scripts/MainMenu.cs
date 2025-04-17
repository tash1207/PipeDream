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

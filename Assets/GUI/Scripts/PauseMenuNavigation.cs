using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuNavigation : MonoBehaviour
{

    public GameObject pauseMenu;


    public void OpenPauseMenu()
    {
        Game.PauseGame();
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Game.ResumeGame();
        pauseMenu.SetActive(false);
    }


    public void GoToMainMenu()
    {
        Game.ResumeGame();
        SceneManager.LoadScene("Main menu");

    }
    public void Restart()
    {
        Game.ResumeGame();
        Game.Instance.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Quit()
    {
        Game.ResumeGame();
        Application.Quit();
    }

}

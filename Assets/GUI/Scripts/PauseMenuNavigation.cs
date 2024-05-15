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
        ResourceGenerator.readPrefabLevel = false;
        Game.ResumeGame();
        SceneManager.LoadScene("Main menu");

    }
    public void Restart()
    {
        ResourceGenerator.readPrefabLevel = false;
        Game.ResumeGame();
        Game.Instance.SetupGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void RestartAndSwitchSpots()
    {
        ResourceGenerator.readPrefabLevel = true;
        // swap PlayerPrefs.GetString("player_1_script_path") and PlayerPrefs.GetString("player_2_script_path")
        string temp = PlayerPrefs.GetString("player_1_script_path");
        PlayerPrefs.SetString("player_1_script_path", PlayerPrefs.GetString("player_2_script_path"));
        PlayerPrefs.SetString("player_2_script_path", temp);
        // swap PlayerPrefs.GetString("player1_name") and PlayerPrefs.GetString("player2_name")
        temp = PlayerPrefs.GetString("player1_name");
        PlayerPrefs.SetString("player1_name", PlayerPrefs.GetString("player2_name"));
        PlayerPrefs.SetString("player2_name", temp);
        
        Game.ResumeGame();
        Game.Instance.SetupGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Quit()
    {
        Game.ResumeGame();
        Application.Quit();
    }

}

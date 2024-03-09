using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;

public class PlayerVsPlayerButtonClick : MonoBehaviour
{
    public void StartGamePlayerVsPlayer()
    {
        if (Game.Instance != null)
        {
            Game.Instance.ResetGame();
        } 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

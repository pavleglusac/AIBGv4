using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVsPlayerButtonClick : MonoBehaviour
{
    public void StartGamePlayerVsPlayer()
    {
        Game.Instance.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

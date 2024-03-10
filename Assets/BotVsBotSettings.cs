using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


using SimpleFileBrowser;
using TMPro;

public class BotVsBotSettings : MonoBehaviour
{

  public Text ButtonTextPlayer1;
  public Text ButtonTextPlayer2;
  public TMP_InputField InputPlayer1;
  public TMP_InputField InputPlayer2;

  public void SetPlayer1Script()
  {
    FileBrowser.ShowLoadDialog(
                (paths) =>
                {
                  PlayerPrefs.SetString("player_1_script_path", paths[0]);
                  ButtonTextPlayer1.text = paths[0];
                  Debug.Log("Selected: " + paths[0]);
                },
                () => { Debug.Log("Canceled"); },
                FileBrowser.PickMode.Files,
                false,
                null,
                null,
                "Select File",
                "Select"
            );

  }

  public void SetPlayer2Script()
  {
    FileBrowser.ShowLoadDialog(
                  (paths) =>
                  {
                    PlayerPrefs.SetString("player_2_script_path", paths[0]);
                    ButtonTextPlayer2.text = paths[0];
                    Debug.Log("Selected: " + paths[0]);
                  },
                  () => { Debug.Log("Canceled"); },
                  FileBrowser.PickMode.Files,
                  false,
                  null,
                  null,
                  "Select File",
                  "Select"
              );

  }

  public bool CheckIfCanBeginGame()
  {
    if (string.IsNullOrEmpty(InputPlayer1.text))
      return false;
    if (string.IsNullOrEmpty(InputPlayer2.text))
      return false;
    if (string.IsNullOrEmpty(PlayerPrefs.GetString("player_1_script_path")))
      return false;
    if (string.IsNullOrEmpty(PlayerPrefs.GetString("player_2_script_path")))
      return false;
    return true;
  }

  public void BeginGame()
  {
    if (!CheckIfCanBeginGame())
    {
      return;
    }
    if (Game.Instance != null)
    {
      Game.Instance.ResetGame();
    }

    PlayerPrefs.SetString("player1_name", InputPlayer1.text);
    PlayerPrefs.SetString("player2_name", InputPlayer2.text);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

  }


}

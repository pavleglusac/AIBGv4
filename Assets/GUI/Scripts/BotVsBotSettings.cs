using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


using SimpleFileBrowser;
using TMPro;
using System.IO;

public class BotVsBotSettings : MonoBehaviour
{

    public Text ButtonTextPlayer1;
    public Text ButtonTextPlayer2;
    public TMP_InputField InputPlayer1;
    public TMP_InputField InputPlayer2;
    public bool LoadGameBot = false;

    public void SetPlayer1Script()
    {
        FileBrowser.SetDefaultFilter( ".sh" );
        FileBrowser.SetFilters(true, new FileBrowser.Filter( "Shell Files", ".sh"));
        FileBrowser.ShowLoadDialog(
                    (paths) =>
                    {
                        PlayerPrefs.SetString("player_1_script_path", paths[0]);
                        string fileName = System.IO.Path.GetFileName(paths[0]);
                        ButtonTextPlayer1.text = fileName;
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
        FileBrowser.SetFilters(true, new FileBrowser.Filter( "Shell Files", ".sh"));
        FileBrowser.ShowLoadDialog(
                      (paths) =>
                      {
                          PlayerPrefs.SetString("player_2_script_path", paths[0]);
                          string fileName = System.IO.Path.GetFileName(paths[0]);
                          ButtonTextPlayer2.text = fileName;
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
        if (string.IsNullOrEmpty(InputPlayer1?.text))
            return false;
        if (string.IsNullOrEmpty(InputPlayer2?.text) && !LoadGameBot)
            return false;
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("player_1_script_path")))
            return false;
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("player_2_script_path")) && !LoadGameBot)
            return false;
        return true;
    }

    public void BeginGame()
    {
        if (!CheckIfCanBeginGame())
        {
            Debug.Log("cant start game becayse reasons");
            return;
        }
        

        if (LoadGameBot)
        {
            PlayerPrefs.SetString("player2_name", "Topic Team");
            string gameBotPath = Path.Combine(Application.streamingAssetsPath, "run.sh");
            PlayerPrefs.SetString("player_2_script_path", gameBotPath);
        } else {
            PlayerPrefs.SetString("player2_name", InputPlayer2.text);
        }

        
        PlayerPrefs.SetString("player1_name", InputPlayer1.text);
        if (Game.Instance != null)
        {
            Game.Instance.ResetGame();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


}

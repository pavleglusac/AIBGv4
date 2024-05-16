using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


using SimpleFileBrowser;
using TMPro;

public class PanelSwitcher : MonoBehaviour
{

    public GameObject FromPanel;
    public GameObject ToPanel;


    public void Switch()
    {
        FromPanel.SetActive(false);
        ToPanel.SetActive(true);
    }

}

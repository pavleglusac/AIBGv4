
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsHandle : MonoBehaviour
{

    public static PlayerStatsHandle Instance;
    [SerializeField] public Text player1Name;
    [SerializeField] public Text player1Coins;
    [SerializeField] public Text player1Energy;
    [SerializeField] public Text player1XP;
    [SerializeField] public Text player1Backpack;

    [SerializeField] public Text player2Name;
    [SerializeField] public Text player2Coins;
    [SerializeField] public Text player2Energy;
    [SerializeField] public Text player2XP;
    [SerializeField] public Text player2Backpack;
    [SerializeField] public Text turnCount;
    [SerializeField] public Text displayMessage;
    [SerializeField] public GameObject displayMessageBackgorund;
    [SerializeField] public GameObject GameOverMenu;
    [SerializeField] public Text displayWinner;
    [SerializeField] public Text displayGameOverReason;


    [SerializeField] public Text Player1MyTurn;
    [SerializeField] public Text Player2MyTurn;

    private bool GameStarted = false;



    public void UpdateGUI(Player Player1, Player Player2, int turnNumber, bool FirstPlayerTurn, string DisplayMessage, bool previousTurnFirstPlayer)
    {
        player1Name.text = Player1.Name.ToString();
        player1Coins.text = Player1.Coins.ToString();
        player1Energy.text = Player1.Energy.ToString();
        player1XP.text = Player1.XP.ToString();
        player1Backpack.text = Player1.Bag.GetWeight().ToString() + "/" + Player1.Bag.Capacity.ToString();

        player2Name.text = Player2.Name.ToString();
        player2Coins.text = Player2.Coins.ToString();
        player2Energy.text = Player2.Energy.ToString();
        player2XP.text = Player2.XP.ToString();
        player2Backpack.text = Player2.Bag.GetWeight().ToString() + "/" + Player2.Bag.Capacity.ToString();

        turnCount.text = turnNumber.ToString();
        displayMessage.text = DisplayMessage;

        if (FirstPlayerTurn)
        {
            Player1MyTurn.text = "My turn!";
            Player2MyTurn.text = "Stand By";

        }
        else
        {
            Player1MyTurn.text = "Stand By";
            Player2MyTurn.text = "My turn!";
        }

        if (GameStarted)
        {
            if (previousTurnFirstPlayer)
                displayMessageBackgorund.GetComponent<Image>().color = new Color(76f / 255f, 88f / 255f, 195f / 255f, 157f / 255f);
            else
                displayMessageBackgorund.GetComponent<Image>().color = new Color(195f / 255f, 76f / 255f, 76f / 255f, 157f / 255f);
        }

        if (!GameStarted)
        {
            GameStarted = true;
        }


    }

    public void GameOverScreen(string gameOverMessage)
    {
        displayWinner.text = "Winner: " + Game.Instance.Winner;
        displayGameOverReason.text = gameOverMessage;
        GameOverMenu.SetActive(true);

    }

    void Update()
    {

    }
    void Start()
    {
        Instance = this;
    }


}

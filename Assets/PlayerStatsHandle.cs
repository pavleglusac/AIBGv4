
using System;
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



    public void UpdateGUI(Player Player1, Player Player2, int turnNumber, bool FirstPlayerTurn, string DisplayMessage)
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

    }

    void Update()
    {

    }
    void Start()
    {
        Instance = this;
    }


}

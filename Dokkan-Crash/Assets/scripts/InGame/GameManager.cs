using UnityEngine;
using TMPro; // TextMeshProの名前空間を追加

public class GameManager : MonoBehaviour
{
    public Swordman[] players; 
    public TextMeshProUGUI turnText; // TextをTextMeshProUGUIに変更

    private int currentPlayerIndex = 0;
    private bool isGameOver = false;

    public int CurrentPlayerIndex
    {
        get { return currentPlayerIndex; }
    }

    private void Start()
    {
        UpdateTurnUI();
        StartTurn();
    }

    private void StartTurn()
    {
        if (!isGameOver)
        {
            players[currentPlayerIndex].StartTurn();
        }
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].EndTurn();
        
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        UpdateTurnUI();
        
        Invoke("StartTurn", 1f); 
    }

    private void UpdateTurnUI()
    {
        turnText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
    }

    public void CheckGameOver()
    {
        int activePlayers = 0;
        foreach (Swordman player in players)
        {
            if (player.IsAlive)
                activePlayers++;
        }

        if (activePlayers <= 1)
        {
            isGameOver = true;
            turnText.text = "Game Over";
        }
    }
}

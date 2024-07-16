using UnityEngine;

public class InGame_GM : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;
    public PlayerManager playerManager;

    private bool isPlayer1Turn = true;

    void Start()
    {
        player1.SetPlayer(true); // 最初はPlayer1のターン
        player2.SetPlayer(false);
    }

    void Update()
    {
        if (isPlayer1Turn)
        {
            player1.HandlePlayerInput();
        }
        else
        {
            player2.HandlePlayerInput();
        }
    }

    public void SwitchTurn()
    {
        playerManager.SwitchToNextTurn();
    }
}

using UnityEngine;

public class InGame_GM : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;

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
        isPlayer1Turn = !isPlayer1Turn;
        player1.SetPlayer(isPlayer1Turn);
        player2.SetPlayer(!isPlayer1Turn);
    }
}

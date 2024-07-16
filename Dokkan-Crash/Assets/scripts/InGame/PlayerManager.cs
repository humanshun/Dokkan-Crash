using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameConst.GameState currentState;
    public void SwitchToNextTurn()
    {
        switch (currentState)
        {
            case GameConst.GameState.PLAYER1TURN:
                SetCurrentTurn(GameConst.GameState.PLAYER2TURN);
                break;
            case GameConst.GameState.PLAYER2TURN:
                SetCurrentTurn(GameConst.GameState.PLAYER1TURN);
                break;
            default:
                SetCurrentTurn(GameConst.GameState.PLAYER1TURN);
                break;
        }
    }
    public void SetCurrentTurn(GameConst.GameState newTurn)
    {
        currentState = newTurn;
        Debug.Log(newTurn);
    }
}

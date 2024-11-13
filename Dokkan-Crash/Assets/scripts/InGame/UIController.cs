using UnityEngine;
using TMPro; // TextMeshProの名前空間を追加

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI healthText; // TextをTextMeshProUGUIに変更
    public TextMeshProUGUI turnText;   // TextをTextMeshProUGUIに変更
    
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        UpdateHealthUI();
        UpdateTurnUI();
    }

    private void UpdateHealthUI()
    {
        PlayerMovement currentPlayer = gameManager.players[gameManager.CurrentPlayerIndex];
        healthText.text = "Health: " + currentPlayer.health;
    }

    private void UpdateTurnUI()
    {
        turnText.text = "Player " + (gameManager.CurrentPlayerIndex + 1) + "'s Turn";
    }
}

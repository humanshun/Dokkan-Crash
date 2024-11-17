using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField; // Input field for entering player names
    [SerializeField] private Button addButton;
    [SerializeField] private TextMeshProUGUI playerListText; // Text for displaying the player list

    private void Start()
    {
        addButton.onClick.AddListener(AddPlayer);
    }

    public void AddPlayer()
    {
        string playerName = nameInputField.text;

        // Ignore if the input is empty
        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("Please enter a player name!");
            return;
        }

        // Add the player name to the list
        SettingsManager.Instance.playerNames.Add(playerName);
        Debug.Log($"Player '{playerName}' has been added.");

        // Update the player list display
        UpdatePlayerList();

        // Clear the input field
        nameInputField.text = "";
    }

    private void UpdatePlayerList()
    {
        // Convert the player list to text and display it
        playerListText.text = "Player List:\n";
        foreach (string name in SettingsManager.Instance.playerNames)
        {
            playerListText.text += $"- {name}\n";
        }
    }
}

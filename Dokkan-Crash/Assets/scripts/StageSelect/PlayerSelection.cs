using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public void SetPlayerCount(int count)
    {
        SettingsManager.Instance.playerCount = count;
    }
}


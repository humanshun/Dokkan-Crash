[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int winCount;

    public PlayerData(string name)
    {
        playerName = name;
        winCount = 0;
    }
}
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int winCount;
    public int killCount;

    public PlayerData(string name)
    {
        playerName = name;
        winCount = 0;
        killCount = 0;
    }
}
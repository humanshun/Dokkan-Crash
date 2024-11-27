[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int point;
    public int winCount;
    public int killCount;

    public PlayerData(string name)
    {
        playerName = name;
        point = 0;
        winCount = 0;
        killCount = 0;
    }
}
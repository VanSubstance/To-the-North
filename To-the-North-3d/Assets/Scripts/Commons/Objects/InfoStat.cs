[System.Serializable]
public class InfoStat
{
    public string text { get; set; }
    public InfoType type { get; set; }

    public InfoStat()
    {
        text = string.Empty;
        type = InfoType.NORMAL;
    }

    public InfoStat(string text, InfoType type = InfoType.NORMAL)
    {
        this.text = text;
        this.type = type;
    }
}
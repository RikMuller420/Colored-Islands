using YG;

public class SaveProvider
{
    public void Save(string saveData)
    {
        YG2.saves.GameProgress = saveData;
        YG2.SaveProgress();
    }

    public string Load()
    {
        return YG2.saves.GameProgress;
    }
}

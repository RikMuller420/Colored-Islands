using UnityEngine;

public class GameProgressSerializer
{
    public string Serialize(GameProgress data)
    {
        return JsonUtility.ToJson(data);
    }

    public GameProgress Deserialize(string json)
    {
        return JsonUtility.FromJson<GameProgress>(json);
    }
}

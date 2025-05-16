using Newtonsoft.Json;

public class GameProgressSerializer
{
    public string Serialize(GameProgress data)
    {
        return JsonConvert.SerializeObject(data);
    }

    public GameProgress Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<GameProgress>(json);
    }
}

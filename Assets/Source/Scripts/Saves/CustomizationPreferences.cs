using System;
using Newtonsoft.Json;

[Serializable]
public class CustomizationPreferences
{
    [JsonProperty] private int _faceId;
    [JsonProperty] private int _hatId;

    [JsonConstructor]
    public CustomizationPreferences(int faceId, int hatId)
    {
        _faceId = faceId;
        _hatId = hatId;
    }

    [JsonIgnore] public int FaceId => _faceId;
    [JsonIgnore] public int HatId => _hatId;
}

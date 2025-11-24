using System;
using Newtonsoft.Json;

[Serializable]
public class CustomizationPreferences
{
    [JsonProperty] private int _faceId;
    [JsonProperty] private int _hatId;
    [JsonProperty] private int _ñolorSampleId;

    [JsonConstructor]
    public CustomizationPreferences(int faceId, int hatId, int ñolorSampleId)
    {
        _faceId = faceId;
        _hatId = hatId;
        _ñolorSampleId = ñolorSampleId;
    }

    public CustomizationPreferences(int faceId, int hatId, ColorSample colorSample)
    {
        _faceId = faceId;
        _hatId = hatId;
        _ñolorSampleId = (int)colorSample;
    }

    [JsonIgnore] public int FaceId => _faceId;
    [JsonIgnore] public int HatId => _hatId;
    [JsonIgnore] public ColorSample ColorSample => (ColorSample)_ñolorSampleId;
}

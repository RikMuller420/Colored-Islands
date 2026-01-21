using System;
using Newtonsoft.Json;

namespace SlimeGround.Data.Saves
{
	[Serializable]
	public class CustomizationPreferences
	{
	    [JsonProperty] private int _faceId;
	    [JsonProperty] private int _hatId;
	    [JsonProperty] private int _colorSampleId;

	    [JsonConstructor]
	    public CustomizationPreferences(int faceId, int hatId, int colorSampleId)
	    {
	        _faceId = faceId;
	        _hatId = hatId;
	        _colorSampleId = colorSampleId;
	    }

	    public CustomizationPreferences(int faceId, int hatId, ColorSample colorSample)
	    {
	        _faceId = faceId;
	        _hatId = hatId;
	        _colorSampleId = (int)colorSample;
	    }

	    [JsonIgnore] public int FaceId => _faceId;
	    [JsonIgnore] public int HatId => _hatId;
	    [JsonIgnore] public ColorSample ColorSample => (ColorSample)_colorSampleId;
	}

}

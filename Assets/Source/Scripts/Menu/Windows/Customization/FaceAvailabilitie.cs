using System;
using Newtonsoft.Json;

namespace SlimeGround.Menu.Windows.Customization
{

	[Serializable]
	public class FaceAvailabilitie
	{
	    [JsonProperty] private int _faceId;
	    [JsonProperty] private bool _isAviable;
	    [JsonProperty] private bool _wasUsed;

	    [JsonConstructor]
	    public FaceAvailabilitie(int faceId, bool isAviable, bool wasUsed)
	    {
	        _faceId = faceId;
	        _isAviable = isAviable;
	        _wasUsed = wasUsed;
	    }

	    [JsonIgnore] public int FaceId => _faceId;
	    [JsonIgnore] public bool IsAviable => _isAviable;
	    [JsonIgnore] public bool WasUsed => _wasUsed;
	}

}

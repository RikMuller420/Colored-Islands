using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class ImageLoader : MonoBehaviour
	{
	    [SerializeField] private Image _image;
	    [SerializeField] private Sprite _defaultImage;

	    public void SetImage(string url)
	    {
	        if (gameObject.activeSelf == false)
	        {
	            SetDefaultImage();
	        }

	        StartCoroutine(SetImageCoroutine(url));
	    }

	    public void SetDefaultImage()
	    {
	        _image.sprite = _defaultImage;
	    }

	    private IEnumerator SetImageCoroutine(string url)
	    {
	        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
	        {
	            yield return request.SendWebRequest();

	            if (request.result == UnityWebRequest.Result.Success)
	            {
	                Texture2D texture = DownloadHandlerTexture.GetContent(request);
	                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	                _image.sprite = sprite;
	            }
	            else
	            {
	                SetDefaultImage();
	            }
	        }
	    }
	}
}

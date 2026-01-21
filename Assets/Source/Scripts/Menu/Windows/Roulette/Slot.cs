using System.Linq;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class Slot : MonoBehaviour
	{
	    [SerializeField] private GameObject _goldIcon;
	    [SerializeField] private TextMeshProUGUI _goldAmountText;
	    [SerializeField] private GameObject _removeAdsIcon;
	    [SerializeField] private GameObject _faceIcon;
	    [SerializeField] private Image _faceImage;

	    private UnitsFaceSettings _faceSettings;
	    private float _faceDropChance = 50;
	    private float _removeAddDropChance = 1;
	    private float _goldPerCoinDropChance = 3000;

	    public float DropChance { get; private set; }
	    public RouletteRewardType RouletteRewardType { get; private set; }
	    public int GoldAmount { get; private set; }
	    public int FaceID { get; private set; }

	    public void Initialize(UnitsFaceSettings faceSettings)
	    {
	        _faceSettings = faceSettings;
	    }

	    public void ActivateGoldIcon(int amount)
	    {
	        DeactivateIcons();

	        _goldIcon.SetActive(true);
	        _goldAmountText.gameObject.SetActive(true);
	        _goldAmountText.text = amount.ToString();
	        DropChance = _goldPerCoinDropChance / amount;
	        RouletteRewardType = RouletteRewardType.Gold;
	        GoldAmount = amount;
	    }

	    public void ActivateRemoveAdsIcon()
	    {
	        DeactivateIcons();
	        _removeAdsIcon.SetActive(true);
	        DropChance = _removeAddDropChance;
	        RouletteRewardType = RouletteRewardType.RemoveAds;
	    }

	    public void ActivateFaceIcon(int faceId)
	    {
	        DeactivateIcons();
	        _faceIcon.SetActive(true);
	        Sprite faceSprite = _faceSettings.Faces.FirstOrDefault(face => face.Id == faceId).Sprite;
	        _faceImage.sprite = faceSprite;
	        DropChance = _faceDropChance;
	        RouletteRewardType = RouletteRewardType.Face;
	        FaceID = faceId;
	    }

	    private void DeactivateIcons()
	    {
	        _goldIcon.SetActive(false);
	        _goldAmountText.gameObject.SetActive(false);
	        _removeAdsIcon.SetActive(false);
	        _faceIcon.SetActive(false);
	    }
	}
}

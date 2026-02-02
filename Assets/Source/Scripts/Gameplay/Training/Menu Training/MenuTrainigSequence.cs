using DG.Tweening;
using Lean.Localization;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.Extensions.Windows;
using SlimeGround.Menu.Windows.Customization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Gameplay.Training
{
	public class MenuTrainigSequence : MonoBehaviour
	{
	    private const string CustomizationHintKey = "Customization Description";
	    private const string ShopHintKey = "Shop Description";
	    private const string FinalHintKey = "Final Training Hint";

	    [SerializeField] private LevelLoader _levelLoader;
	    [SerializeField] private Image _fullDimImage;
	    [SerializeField] private CustomizationWindow _customizationWindow;
	    [SerializeField] private MenuWindow _shopWindow;

	    [SerializeField] private CanvasGroup _trainingHintGroup;
	    [SerializeField] private TextMeshProUGUI _trainingHintText;
	    [SerializeField] private Button _dimButton;
	    [SerializeField] private Button _goNextButton;

	    private float _startDelay = 0.3f;
	    private float _fadeDuration = 1f;
	    private float _descriptionTypeDuration = 1.7f;

	    public void StartTraining()
	    {
	        _fullDimImage.raycastTarget = true;
	        _fullDimImage.color = Color.black;
	        _levelLoader.LoadMainMenu();
	        _customizationWindow.Open();
	        DOTween.Sequence().Append(_fullDimImage.DOFade(0f, _fadeDuration)
	                  .SetDelay(_startDelay)
	                  .SetEase(Ease.InOutQuad))
	                  .OnComplete(() => StartCustomizationTraining());
	    }

	    private void StartCustomizationTraining()
	    {
	        _trainingHintGroup.blocksRaycasts = true;
	        DOTween.Sequence().Append(_trainingHintGroup.DOFade(1f, _fadeDuration));
	        PrintHint(CustomizationHintKey);

	        _goNextButton.interactable = false;
	        _goNextButton.onClick.AddListener(GoToShopTraining);

	        _dimButton.interactable = false;
	        _dimButton.onClick.AddListener(GoToShopTraining);
	    }

	    private void PrintHint(string key)
	    {
	        string description = LeanLocalization.GetTranslationText(key);
	        _trainingHintText.text = "";
	        DOTween.Sequence().Append(_trainingHintText.DOText(description, _descriptionTypeDuration)
	                          .SetEase(Ease.Linear))
	                          .OnComplete(() =>
	                          {
	                              _goNextButton.interactable = true;
	                              _dimButton.interactable = true;
	                          });
	    }

	    private void GoToShopTraining()
	    {
	        _goNextButton.interactable = false;
	        _goNextButton.onClick.RemoveListener(GoToShopTraining);
	        _dimButton.interactable = false;
	        _dimButton.onClick.RemoveListener(GoToShopTraining);

	        _customizationWindow.Close();
	        _shopWindow.Open();
	        PrintHint(ShopHintKey);

	        _goNextButton.onClick.AddListener(StartFinalHintTraining);
	        _dimButton.onClick.AddListener(StartFinalHintTraining);
	    }

	    private void StartFinalHintTraining()
	    {
	        _goNextButton.interactable = false;
	        _goNextButton.onClick.RemoveListener(StartFinalHintTraining);
	        _dimButton.interactable = false;
	        _dimButton.onClick.RemoveListener(StartFinalHintTraining);

	        _shopWindow.Close();
	        PrintHint(FinalHintKey);

	        _goNextButton.onClick.AddListener(CloseTraining);
	        _dimButton.onClick.AddListener(CloseTraining);
	    }

	    private void CloseTraining()
	    {
	        _fullDimImage.raycastTarget = false;
	        _goNextButton.onClick.RemoveListener(CloseTraining);
	        _dimButton.onClick.RemoveListener(CloseTraining);

	        DOTween.Sequence().Append(_trainingHintGroup.DOFade(0f, _fadeDuration));
	        _trainingHintGroup.blocksRaycasts = false;
	    }
	}
}

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.FinalScore
{
	public class ObjectiveAnimator : MonoBehaviour
	{
		private const float StartAppearScale = 1.2f;
		private const float ScaleChangeDuration = 0.5f;
		private const float FadeDuration = 0.5f;
		private const float RotationAngle = 8f;
		private const float RotationStepDuration = 0.35f;
		private const float ColorChangeDuration = 0.25f;

		private readonly Color _reachedColor = Color.white;
		private readonly Color _failedColor = new Color(0.92f, 0.55f, 0.55f);

		[SerializeField] private TextMeshProUGUI _text;
	    [SerializeField] private CanvasGroup _canvasGroup;
	    [SerializeField] private Image _icon;

	    private Quaternion _leftRotation;
	    private Quaternion _rightRotation;
	    private Quaternion _defaultRotation;

	    public float AnimationDuration { get => StartAppearScale; }

	    private void Awake()
	    {
	        _leftRotation = Quaternion.Euler(new Vector3(0, 0, RotationAngle));
	        _rightRotation = Quaternion.Euler(new Vector3(0, 0, -RotationAngle));
	        _defaultRotation = Quaternion.Euler(Vector3.zero);
	    }

	    public void ResetObjective()
	    {
	        _canvasGroup.alpha = 0;
	        _icon.color = _reachedColor;
	        _text.fontStyle &= ~FontStyles.Strikethrough;
	    }

	    public void ShowAppearAnimation(string text, bool isReached)
	    {
	        _text.text = text;

	        transform.localScale = Vector3.one * StartAppearScale;
	        _canvasGroup.alpha = 0f;

	        transform.DOScale(1f, ScaleChangeDuration);
	        _canvasGroup.DOFade(1f, FadeDuration);

	        Sequence scaleSequence = DOTween.Sequence();
	        scaleSequence.Append(transform.DORotateQuaternion(_leftRotation, RotationStepDuration))
	                     .Append(transform.DORotateQuaternion(_rightRotation, RotationStepDuration))
	                     .Append(transform.DORotateQuaternion(_leftRotation, RotationStepDuration))
	                     .Append(transform.DORotateQuaternion(_defaultRotation, RotationStepDuration));

	        if (isReached == false)
	        {
	            Sequence unreachSequence = DOTween.Sequence();

	            unreachSequence.AppendInterval(ScaleChangeDuration)
	                .AppendCallback(() => _text.fontStyle |= FontStyles.Strikethrough)
	                .Join(_icon.DOColor(_failedColor, ColorChangeDuration));
	        }
	    }
	}
}

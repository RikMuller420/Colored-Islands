using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.FinalScore
{
	public class ObjectiveAnimator : MonoBehaviour
	{
	    [SerializeField] private TextMeshProUGUI _text;
	    [SerializeField] private CanvasGroup _canvasGroup;
	    [SerializeField] private Image _icon;

	    private float _startAppearScale = 1.2f;
	    private float _scaleChangeDuration = 0.5f;
	    private float _fadeDuration = 0.5f;
	    private float _rotationAngle = 8f;
	    private float _rotationStepDuration = 0.35f;

	    private Color _reachedColor = Color.white;
	    private Color _failedColor = new Color(0.92f, 0.55f, 0.55f);
	    private float _colorChangeDuration = 0.25f;

	    private Quaternion _leftRotation;
	    private Quaternion _rightRotation;
	    private Quaternion _defaultRotation;

	    public float AnimationDuration { get => _startAppearScale; }

	    private void Awake()
	    {
	        _leftRotation = Quaternion.Euler(new Vector3(0, 0, _rotationAngle));
	        _rightRotation = Quaternion.Euler(new Vector3(0, 0, -_rotationAngle));
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

	        transform.localScale = Vector3.one * _startAppearScale;
	        _canvasGroup.alpha = 0f;

	        transform.DOScale(1f, _scaleChangeDuration);
	        _canvasGroup.DOFade(1f, _fadeDuration);

	        Sequence scaleSequence = DOTween.Sequence();
	        scaleSequence.Append(transform.DORotateQuaternion(_leftRotation, _rotationStepDuration))
	                     .Append(transform.DORotateQuaternion(_rightRotation, _rotationStepDuration))
	                     .Append(transform.DORotateQuaternion(_leftRotation, _rotationStepDuration))
	                     .Append(transform.DORotateQuaternion(_defaultRotation, _rotationStepDuration));

	        if (isReached == false)
	        {
	            Sequence unreachSequence = DOTween.Sequence();

	            unreachSequence.AppendInterval(_scaleChangeDuration)
	                .AppendCallback(() => _text.fontStyle |= FontStyles.Strikethrough)
	                .Join(_icon.DOColor(_failedColor, _colorChangeDuration));
	        }
	    }
	}
}

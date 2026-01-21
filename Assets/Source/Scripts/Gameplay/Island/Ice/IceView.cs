using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	public class IceView : MonoBehaviour
	{
	    [SerializeField] private MeshRenderer _iceMeshRenderer;
	    [SerializeField] private CanvasGroup _movesCountGroup;
	    [SerializeField] private TextMeshProUGUI _movesToDeactivateText;

	    private Material _iceMaterial;
	    private float _fadeDuration = 1f;

	    private void Awake()
	    {
	        _iceMaterial = new Material(_iceMeshRenderer.material);
	        _iceMeshRenderer.material = _iceMaterial;
	    }

	    public void Activate(Transform cameraTransform)
	    {
	        UpdateTextRotation(cameraTransform);

	        DOTween.Sequence()
	                .Append(_movesCountGroup.DOFade(1f, _fadeDuration).SetEase(Ease.OutQuad));
	    }

	    public void Deactivate()
	    {
	        DOTween.Sequence()
	                .Append(_iceMaterial.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuad))
	                .Join(_movesCountGroup.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuad))
	                .OnComplete(() =>
	                {
	                    _iceMeshRenderer.enabled = false;
	                });
	    }

	    public void SetMovesToDeactivateText(int movesCount)
	    {
	        if (movesCount < 0)
	        {
	            movesCount = 0;
	        }

	        _movesToDeactivateText.text = movesCount.ToString();
	    }

	    private void UpdateTextRotation(Transform lookAtPoint)
	    {
	        _movesCountGroup.transform.LookAt(lookAtPoint);
	    }
	}
}

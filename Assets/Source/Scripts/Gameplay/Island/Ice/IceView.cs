using System.Collections;
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
		[SerializeField] private GameObject _breackParticle;

		private Material _iceMaterial;

		private float _fadeDuration = 1f;
		private float _cameraTrackDuration = 4f;

		private Coroutine _rotateCoroutine;
		private WaitForEndOfFrame _waitForEndOfFrame;

		private void Awake()
	    {
			SetMaterialDublicate(ref _iceMaterial, _iceMeshRenderer);

			_waitForEndOfFrame = new WaitForEndOfFrame();
		}

		private void OnDisable()
		{
			if (_rotateCoroutine != null)
			{
				StopCoroutine(_rotateCoroutine);
			}
		}

		public void Activate(Transform cameraTransform)
	    {
			_rotateCoroutine = StartCoroutine(RotateTextPanel(cameraTransform));

			DOTween.Sequence()
				   .Append(_movesCountGroup.DOFade(1f, _fadeDuration).SetEase(Ease.OutQuad));
	    }

	    public void Deactivate()
	    {
			_breackParticle.SetActive(true);

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

		private IEnumerator RotateTextPanel(Transform lookAtPoint)
		{
			float time = 0f;
			
			while (enabled)
			{
				yield return _waitForEndOfFrame;

				time += Time.deltaTime;
				_movesCountGroup.transform.LookAt(lookAtPoint);

				if (time > _cameraTrackDuration)
				{
					break;
				}
			}
		}

		private void SetMaterialDublicate(ref Material material, MeshRenderer meshRenderer)
		{
			material = new Material(meshRenderer.material);
			meshRenderer.material = material;
		}
	}
}

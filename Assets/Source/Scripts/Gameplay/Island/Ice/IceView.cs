using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	public class IceView : MonoBehaviour
	{
		private const float FadeDuration = 1f;
		private const float CameraTrackDuration = 4f;

		[SerializeField] private MeshRenderer _iceMeshRenderer;
		[SerializeField] private CanvasGroup _movesCountGroup;
	    [SerializeField] private TextMeshProUGUI _movesToDeactivateText;
		[SerializeField] private GameObject _breackParticle;

		private Material _iceMaterial;
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
				   .Append(_movesCountGroup.DOFade(1f, FadeDuration).SetEase(Ease.OutQuad));
	    }

	    public void Deactivate()
	    {
			_breackParticle.SetActive(true);

			DOTween.Sequence()
	                .Append(_iceMaterial.DOFade(0f, FadeDuration).SetEase(Ease.OutQuad))
	                .Join(_movesCountGroup.DOFade(0f, FadeDuration).SetEase(Ease.OutQuad))
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

				Vector3 direction = lookAtPoint.position - _movesCountGroup.transform.position;
				direction.x = 0f;
				_movesCountGroup.transform.rotation = Quaternion.LookRotation(direction);

				time += Time.deltaTime;

				if (time > CameraTrackDuration)
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

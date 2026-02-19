using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Menu.Windows.Customization;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class Unit : MonoBehaviour
	{
	    [SerializeField] private UnitRenderer _renderer;
	    [SerializeField] private Transform _meshTransform;
	    [SerializeField] private UnitAnimator _animator;
	    [SerializeField] private UnitCollider _collider;
	    [SerializeField] private Transform _body;

	    private UnitLookAtRotator _lookAtRotator;
	    private bool _isInitialized;

	    public UnitSlotType Slot { get; private set; }
	    public BaseIsland Island { get; private set; }
	    public Transform MeshTransform => _meshTransform;
	    public UnitAnimator Animator => _animator;

	    public void ActivateOutline() => _renderer.ActivateOutline();
	    public void DeactivateOutline() => _renderer.DeactivateOutline();

	    public void FreezeAnimation() => _animator.FreezeAnimation();
	    private void UnfreezeAnimation() => _animator.UnfreezeAnimation();

	    public void Initialize(CustomizationSettingsHolder customizationSettings)
	    {
	        if (_isInitialized == false)
	        {
	            _renderer.Initialize(customizationSettings);
	            _lookAtRotator = new UnitLookAtRotator(_body);
	            _isInitialized = true;
	        }
	    }

	    public void SetScale(float scale)
	    {
	        _meshTransform.localScale = Vector3.one * scale;
	    }

	    public void SetIsland(BaseIsland island)
	    {
	        Island = island;
	    }

	    public void SetUnitSlot(UnitSlotType slot)
	    {
	        Slot = slot;
	        _renderer.SetPaint(slot);
	    }

	    public void Deactivate()
	    {
	        enabled = false;
	        _collider.Deactivate();
	    }

	    public void Activate()
	    {
	        enabled = true;
	        _collider.Activate();
	        UnfreezeAnimation();
	    }

	    public void LookToTarget(Transform target, UnitsMoveInfo unitsMoveInfo) => 
	                                            _lookAtRotator.LookToTarget(target, unitsMoveInfo);

	    public void ResetRotation() => _lookAtRotator.ResetRotation();

	#if UNITY_EDITOR
	    public void SetMaterial(Material material)
	    {
	        _renderer.SetMaterial(material);
	    }
	#endif
	}
}

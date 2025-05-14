using UnityEngine;
using UnityEngine.UI;

public class ObjectivesFreezeBoostButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private ObjectivesFreezeBoost _objectivesFreezeBoost;

    private void OnEnable()
    {
        _button.onClick.AddListener(ApplyBoost);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ApplyBoost);
    }

    public void Initialize(ObjectivesFreezeBoost objectivesFreezeBoost)
    {
        _objectivesFreezeBoost = objectivesFreezeBoost;
    }

    private void ApplyBoost()
    {
        _objectivesFreezeBoost.FreezeObjectives();
    }
}

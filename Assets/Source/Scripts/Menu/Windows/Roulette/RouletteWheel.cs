using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class RouletteWheel : MonoBehaviour
{
    [SerializeField] private RectTransform _wheel;
    [SerializeField] private List<Slot> _slots = new();

    private int _maxFaceSlots = 4;
    private float _minSpinDuration = 3f;
    private float _maxSpinDuration = 5f;
    private int _minRotations = 3;
    private int _maxRotations = 6;
    private float _slotRotation;
    private float _fullWheelRotation = 360f;
    private float _maxDinishAngleOffset = 10f;
    private int[] _goldInRewards = new int[8]
    {
        100,
        200,
        100,
        200,
        100,
        500,
        100,
        1000
    };
    private Quaternion _whellStartLocalRotation;

    private GameProgressStorage _progressStorage;
    private UpgradesProvider _upgradesProvider;

    public event System.Action SpinStarted;
    public event System.Action<Slot> SpinFinished;


    public void Initialize(GameProgressStorage progressStorage, UnitsFaceSettings faceSettings,
                           UpgradesProvider upgradesProvider)
    {
        _progressStorage = progressStorage;
        _upgradesProvider = upgradesProvider;
        _slotRotation = _fullWheelRotation / _slots.Count;
        _whellStartLocalRotation = _wheel.localRotation;

        foreach (Slot slot in _slots)
        {
            slot.Initialize(faceSettings);
        }
    }

    public void Spin()
    {
        float totalChance = 0f;

        foreach (Slot slot in _slots)
        {
            totalChance += slot.DropChance;
        }

        float randomValue = Random.value * totalChance;
        float currentSum = 0f;
        Slot winningSlot = _slots[0];
        int winningIndex = 0;

        for (int i = 0; i < _slots.Count; i++)
        {
            currentSum += _slots[i].DropChance;

            if (randomValue <= currentSum)
            {
                winningSlot = _slots[i];
                winningIndex = i;

                break;
            }
        }

        float slotAngle = winningIndex * _slotRotation;
        float randomOffset = Random.Range(-_maxDinishAngleOffset, _maxDinishAngleOffset);
        float targetRotation = slotAngle + randomOffset;

        float fullRotations = Random.Range(_minRotations, _maxRotations) * _fullWheelRotation;
        float finalRotation = -(targetRotation + fullRotations);

        float spinDuration = Random.Range(_minSpinDuration, _maxSpinDuration);

        _wheel.DORotate(new Vector3(0, 0, finalRotation), spinDuration, RotateMode.FastBeyond360)
              .SetEase(Ease.InOutQuad)
              .OnStart(() =>
              {
                  SpinStarted?.Invoke();
              })
              .OnComplete(() =>
              {
                  SpinFinished?.Invoke(winningSlot);
              });
    }

    public void PrepareSlots()
    {
        _wheel.localRotation = _whellStartLocalRotation;

        bool isRemoveAdsSlotAviable = _progressStorage.IsAdsRemoved == false;
        List<int> faceIds = AviableFaceIds();
        List<Slot> unusedSlots = new List<Slot>(_slots);

        int faceSlotIndex = 0;

        foreach (int faceId in faceIds)
        {
            unusedSlots[faceSlotIndex].ActivateFaceIcon(faceId);
            unusedSlots.RemoveAt(faceSlotIndex);
            faceSlotIndex++;
        }

        if (isRemoveAdsSlotAviable)
        {
            unusedSlots[0].ActivateRemoveAdsIcon();
            unusedSlots.RemoveAt(0);
        }

        for (int i = 0; i< unusedSlots.Count; i++)
        {
            int goldAmount = _upgradesProvider.CalculateUpgradedGoldAmount(_goldInRewards[i]);

            unusedSlots[i].ActivateGoldIcon(goldAmount);
        }
    }

    private List<int> AviableFaceIds()
    {
        List<int> lockedFaceIds = _progressStorage.FaceAvailabilities
                                                .Where(face => !face.IsAviable)
                                                .Select(face => face.FaceId)
                                                .ToList();
        List<int> randomFaceIds = new List<int>();
        int count = Mathf.Min(_maxFaceSlots, lockedFaceIds.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, lockedFaceIds.Count);
            randomFaceIds.Add(lockedFaceIds[randomIndex]);
            lockedFaceIds.RemoveAt(randomIndex);
        }

        return randomFaceIds;
    }
}

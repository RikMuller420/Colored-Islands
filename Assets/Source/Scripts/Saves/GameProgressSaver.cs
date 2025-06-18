using System;
using System.Collections;
using UnityEngine;

public class GameProgressSaver : MonoBehaviour
{
    private GameProgressSerializer _progressSerializer;
    private SaveProvider _saveProvider;

    private DateTime _lastSaveTime;
    private float _saveCooldown = 3f;
    private Coroutine _saveCorutine;
    private WaitForEndOfFrame _waitForEndOfFrame;

    public void Initialize(SaveProvider saveProvider)
    {
        _lastSaveTime = DateTime.Now;
        _waitForEndOfFrame = new WaitForEndOfFrame();
        _saveProvider = saveProvider;
        _progressSerializer = new GameProgressSerializer();

        enabled = true;
    }

    public void TrySave(GameProgress progress)
    {
        if (IsAbleToSave())
        {
            Save(progress);
        }
        else
        {
            TryStopSaveCorutine();
            _saveCorutine = StartCoroutine(SaveWhileEnabled(progress));
        }
    }

    private IEnumerator SaveWhileEnabled(GameProgress progress)
    {
        while (enabled)
        {
            yield return _waitForEndOfFrame;

            if (IsAbleToSave())
            {
                Save(progress);

                break;
            }
        }
    }

    private void TryStopSaveCorutine()
    {
        if (_saveCorutine != null)
        {
            StopCoroutine(_saveCorutine);
        }
    }

    private bool IsAbleToSave()
    {
        return (DateTime.Now - _lastSaveTime).TotalSeconds > _saveCooldown;
    }

    private void Save(GameProgress progress)
    {
        string json = _progressSerializer.Serialize(progress);
        _saveProvider.Save(json);
        _lastSaveTime = DateTime.Now;
    }
}

using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
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

        enabled = true;
    }

    public void TrySave(PlayerData playerData)
    {
        if (IsAbleToSave())
        {
            Save(playerData);
        }
        else
        {
            TryStopSaveCorutine();
            _saveCorutine = StartCoroutine(SaveWhileEnabled(playerData));
        }
    }

    private IEnumerator SaveWhileEnabled(PlayerData playerData)
    {
        while (enabled)
        {
            yield return _waitForEndOfFrame;

            if (IsAbleToSave())
            {
                Save(playerData);

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

    private void Save(PlayerData playerData)
    {
        string json = JsonConvert.SerializeObject(playerData);
        _saveProvider.Save(json);
        _lastSaveTime = DateTime.Now;
    }
}

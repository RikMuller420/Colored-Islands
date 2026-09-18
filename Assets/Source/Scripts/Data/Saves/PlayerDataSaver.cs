using System;
using System.Collections;
using Newtonsoft.Json;
using SlimeGround.Integration.Saves;
using UnityEngine;

namespace SlimeGround.Data.Saves
{
	public class PlayerDataSaver : MonoBehaviour
	{
		private const float RefreshRate = 0.5f;
		private const float SaveCooldown = 3f;

		private SaveProvider _saveProvider;
	    private DateTime _lastSaveTime;
	    private Coroutine _saveCorutine;
	    private WaitForSeconds _wait;

	    public void Initialize(SaveProvider saveProvider)
	    {
	        _lastSaveTime = DateTime.Now;
			_wait = new WaitForSeconds(RefreshRate);
	        _saveProvider = saveProvider;

	        enabled = true;
	    }

	    public void SaveWhileEnabled(PlayerData playerData)
	    {
	        if (IsAbleToSave())
	        {
	            Save(playerData);
	        }
	        else
	        {
	            StopSaveCorutine();
	            _saveCorutine = StartCoroutine(SaveWhileEnabledCoroutine(playerData));
	        }
	    }

	    private IEnumerator SaveWhileEnabledCoroutine(PlayerData playerData)
	    {
	        while (enabled)
	        {
	            yield return _wait;

	            if (IsAbleToSave())
	            {
	                Save(playerData);

	                break;
	            }
	        }
	    }

	    private void StopSaveCorutine()
	    {
	        if (_saveCorutine != null)
	        {
	            StopCoroutine(_saveCorutine);
	        }
	    }

	    private bool IsAbleToSave()
	    {
	        return (DateTime.Now - _lastSaveTime).TotalSeconds > SaveCooldown;
	    }

	    private void Save(PlayerData playerData)
	    {
	        string json = JsonConvert.SerializeObject(playerData);
	        _saveProvider.Save(json);
	        _lastSaveTime = DateTime.Now;
	    }
	}
}

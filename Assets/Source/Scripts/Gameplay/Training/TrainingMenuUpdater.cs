using System.Collections.Generic;
using SlimeGround.Data.Saves;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	public class TrainingMenuUpdater : MonoBehaviour
	{
	    [SerializeField] private List<GameObject> _trainingDummyPanels;
	    [SerializeField] private List<GameObject> _unlockedPanels;

	    private IPlayerData _playerData;

	    public void Initilize(IPlayerData playerData)
	    {
	        _playerData = playerData;

	        _playerData.Progress.TrainingFinished += UpdateMenuAvaliability;
	        UpdateMenuAvaliability();
	    }

		public void Dispose()
		{
			_playerData.Progress.TrainingFinished -= UpdateMenuAvaliability;
		}

	    private void UpdateMenuAvaliability()
	    {
	        foreach (GameObject panel in _trainingDummyPanels)
	        {
	            panel.SetActive(_playerData.Progress.IsTrainingFinished == false);
	        }

	        foreach (GameObject panel in _unlockedPanels)
	        {
	            panel.SetActive(_playerData.Progress.IsTrainingFinished);
	        }
	    }
	}
}

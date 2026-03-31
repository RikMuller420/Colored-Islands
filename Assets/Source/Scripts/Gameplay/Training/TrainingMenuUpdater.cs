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

	        _playerData.TrainingFinished += UpdateMenuAvaliability;
	        UpdateMenuAvaliability();
	    }

		public void Dispose()
		{
			_playerData.TrainingFinished -= UpdateMenuAvaliability;
		}

	    private void UpdateMenuAvaliability()
	    {
	        foreach (GameObject panel in _trainingDummyPanels)
	        {
	            panel.SetActive(_playerData.IsTrainingFinished == false);
	        }

	        foreach (GameObject panel in _unlockedPanels)
	        {
	            panel.SetActive(_playerData.IsTrainingFinished);
	        }
	    }
	}
}

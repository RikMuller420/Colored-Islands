using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameProgress
{
    [SerializeField] private List<LevelProgress> _levels;
    [SerializeField] private int _scoreAmount;
    [SerializeField] private int _goldAmount;

    public GameProgress()
    {
        _levels = new List<LevelProgress>();
        _scoreAmount = 0;
        _goldAmount = 0;
    }

    public LevelProgress FirstUnfinishedLevel => _levels.FirstOrDefault(level => !level.IsDone);
    public IReadOnlyCollection<LevelProgress> Levels => _levels.AsReadOnly();
    public int ScoreAmount => _scoreAmount;
    public int GoldAmount => _goldAmount;

    public void SetGoldAmount(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(nameof(amount));
        }

        if (_goldAmount == amount)
        {
            return;
        }

        _goldAmount = amount;
    }

    public void SetScoreAmount(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(nameof(amount));
        }

        _scoreAmount = amount;
    }

    public void UpdateLevelProgress(LevelProgress levelProgress)
    {
        int index = _levels.FindIndex(level => level.Id == levelProgress.Id);

        if (index == -1)
        {
            throw new ArgumentException($"Level with ID {levelProgress.Id} not found.");
        }

        _levels[index] = levelProgress;
    }

    public void AddLevel(LevelProgress levelProgress)
    {
        if (levelProgress == null)
        {
            throw new ArgumentNullException(nameof(levelProgress));
        }

        _levels.Add(levelProgress);
    }
}


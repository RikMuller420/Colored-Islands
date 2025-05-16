using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class GameProgress
{
    [JsonProperty] private List<LevelProgress> _levels;
    [JsonProperty] private int _scoreAmount;
    [JsonProperty] private int _goldAmount;
    [JsonProperty] private Dictionary<string, int> _boostsAmounts;

    public GameProgress()
    {
        _levels = new List<LevelProgress>();
        _scoreAmount = 0;
        _goldAmount = 0;
        _boostsAmounts = new Dictionary<string, int>()
        {
            { typeof(IslandFinishBoost).FullName, 0 },
            { typeof(ObjectivesFreezeBoost).FullName, 0 },
            { typeof(BufferIslandBoost).FullName, 0 },
            { typeof(PaintAmountReduceBoost).FullName, 0 }
        };
    }

    [JsonIgnore] public LevelProgress FirstUnfinishedLevel => _levels.FirstOrDefault(level => !level.IsDone);
    [JsonIgnore] public IReadOnlyCollection<LevelProgress> Levels => _levels.AsReadOnly();
    [JsonIgnore] public int ScoreAmount => _scoreAmount;
    [JsonIgnore] public int GoldAmount => _goldAmount;

    public int GetBoostAmount<T>() where T : Boost => _boostsAmounts[typeof(T).FullName];

    public void SetBoostAmount<T>(int amount) where T : Boost
    {
        if (_boostsAmounts.ContainsKey(typeof(T).FullName) == false)
        {
            throw new ArgumentException(nameof(T));
        }

        _boostsAmounts[typeof(T).FullName] = amount;
    }

    public void SetGoldAmount(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(nameof(amount));
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


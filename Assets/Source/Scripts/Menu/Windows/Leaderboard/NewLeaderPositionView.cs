using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Lean.Localization;
using TMPro;
using UnityEngine;

public class NewLeaderPositionView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private List<NewLeaderPositionData> _textDatas = new List<NewLeaderPositionData>();

    [SerializeField] private LeaderboardSynchronizer _leaderboardSynchronizer;
    [SerializeField] private LeaderboardSettings _leaderboardSettings;
    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;

    private Dictionary<LeaderboardType, int> _playerRanks = new ();
    private Dictionary<LeaderboardType, int> _showedPlayerRanks = new();

    private DateTime _lastShowTime = DateTime.Now;

    private int _showCoolDownSeconds = 60;
    private int _startShowLevelId = 8;
    private float _fadeDuration = 0.5f;
    private float _typeTextDuration = 1.5f;
    private float _showDuration = 6f;

    private void OnEnable()
    {
        _leaderboardSynchronizer.PlayerScoreChanged += OnPlayerScoreChanged;
        _levelChangeEventTracker.LevelChanged += TryShowNewRank;
    }

    private void OnDisable()
    {
        _leaderboardSynchronizer.PlayerScoreChanged -= OnPlayerScoreChanged;
        _levelChangeEventTracker.LevelChanged += TryShowNewRank;
    }

    private void OnPlayerScoreChanged(Leaderboard leaderboard)
    {
        LeaderboardType leaderboardType = _leaderboardSettings.LeaderboardType(leaderboard.Key);

        if (_playerRanks.ContainsKey(leaderboardType))
        {
            _playerRanks[leaderboardType] = leaderboard.CurrentPlayerRank;
        }
        else
        {
            _playerRanks.Add(leaderboardType, leaderboard.CurrentPlayerRank);
        }
    }

    private void TryShowNewRank(ILevelData levelData)
    {
        float secondsFromLastShow = (float)DateTime.Now.Subtract(_lastShowTime).TotalSeconds;

        if (levelData.LevelId < _startShowLevelId ||
            secondsFromLastShow < _showCoolDownSeconds ||
            _playerRanks.Count == 0)
        {
            return;
        }

        LeaderboardType leaderboardType = _playerRanks.Keys.First();
        int newRank = _playerRanks[leaderboardType];

        if (_showedPlayerRanks.ContainsKey(leaderboardType) &&
            _showedPlayerRanks[leaderboardType] == newRank)
        {
            _playerRanks.Remove(leaderboardType);
            TryShowNewRank(levelData);

            return;
        }

        NewLeaderPositionData textData = _textDatas.FirstOrDefault(data => data.Type == leaderboardType);
        textData.Token.SetValue(newRank);
        string fullText = LeanLocalization.GetTranslationText(textData.LeanTextKey);
        _text.text = "";
        _text.DOText(fullText, _typeTextDuration).SetEase(Ease.Linear);

        _canvasGroup.DOFade(1f, _fadeDuration).OnComplete(() =>
        {
            DOVirtual.DelayedCall(_showDuration, () =>
            {
                _canvasGroup.DOFade(0f, _fadeDuration);
            });
        });

        _lastShowTime = DateTime.Now;

        if (_showedPlayerRanks.ContainsKey(leaderboardType))
        {
            _showedPlayerRanks[leaderboardType] = newRank;
        }
        else
        {
            _showedPlayerRanks.Add(leaderboardType, newRank);
        }

        _playerRanks.Remove(leaderboardType);
    }
}

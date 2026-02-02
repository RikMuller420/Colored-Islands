using System.Globalization;
using Lean.Localization;
using TMPro;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class PlayerResultView : MonoBehaviour
	{
	    private const string RankPrefix = "#";
	    private const string EmptyPlayerLocalizationKey = "Free Place";
	    private const string ZeroPlayerScore = "0";
	    private const string MaxPlayerScore = "Absolute";

		private readonly string[] _scoreSuffixes = { "", "K", "M", "B", "T" };

		[SerializeField] private TextMeshProUGUI _playerRank;
	    [SerializeField] private TextMeshProUGUI _playerName;
	    [SerializeField] private TextMeshProUGUI _playerScore;
	    [SerializeField] private GameObject _currentPlayerHighlight;
	    [SerializeField] private ImageLoader imageLoader;

	    public void SetPlayeData(LeaderboardPlayerData player, bool isCurrentPlayer)
	    {
	        _playerRank.text = $"{RankPrefix}{player.Rank}";
	        _playerName.text = player.Name;
	        _playerScore.text = FormatScore(player.Score);
	        _currentPlayerHighlight.SetActive(isCurrentPlayer);
	        imageLoader.SetImage(player.PhotoLink);
	    }

	    public void SetEmptyPlayerData()
	    {
	        _playerRank.text = RankPrefix;
	        _playerName.text = LeanLocalization.GetTranslationText(EmptyPlayerLocalizationKey);
	        _playerScore.text = ZeroPlayerScore;
	        _currentPlayerHighlight.SetActive(false);
	        imageLoader.SetDefaultImage();
	    }

	    private string FormatScore(int score)
	    {
	        if (score == 0)
	        {
	            return ZeroPlayerScore;
	        }

	        int suffixIndex = 0;
	        int buferScore = score;

	        while (buferScore >= 1000 && suffixIndex < _scoreSuffixes.Length - 1)
	        {
	            buferScore /= 1000;
	            suffixIndex++;
	        }

	        if (suffixIndex >= _scoreSuffixes.Length)
	        {
	            return MaxPlayerScore;
	        }

	        float adjustedScore = score / (float)Mathf.Pow(1000, suffixIndex);

	        if (suffixIndex > 0)
	        {
	            return $"{adjustedScore.ToString("F1", CultureInfo.InvariantCulture)}{_scoreSuffixes[suffixIndex]}"
	                    .Replace(".0", "");
	        }

	        return score.ToString();
	    }
	}
}

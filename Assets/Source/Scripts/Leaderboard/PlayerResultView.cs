using Lean.Localization;
using TMPro;
using UnityEngine;

public class PlayerResultView : MonoBehaviour
{
    private const string RankPrefix = "#";
    private const string EmptyPlayerLocalizationKey = "Free Place";
    private const string EmptyPlayerScore = "0";

    [SerializeField] private TextMeshProUGUI _playerRank;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private GameObject _currentPlayerHighlight;
    [SerializeField] private ImageLoader imageLoader;

    public void SetPlayeData(LeaderboardPlayerData player, bool isCurrentPlayer)
    {
        _playerRank.text = $"{RankPrefix}{player.Rank}";
        _playerName.text = player.Name;
        _playerScore.text = player.Score.ToString();
        _currentPlayerHighlight.SetActive(isCurrentPlayer);
        imageLoader.SetImage(player.PhotoLink);
    }

    public void SetEmptyPlayerData()
    {
        _playerRank.text = RankPrefix;
        _playerName.text = LeanLocalization.GetTranslationText(EmptyPlayerLocalizationKey);
        _playerScore.text = EmptyPlayerScore;
        _currentPlayerHighlight.SetActive(false);
        imageLoader.SetDefaultImage();
    }
}

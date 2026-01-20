using System.Collections.Generic;
using UnityEngine;

public class SettingsWindowInitializer : MonoBehaviour
{
    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private LevelRewardSettings _levelRewardSettings;
    [SerializeField] private AudioMixers _audioMixers;

    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;

    [SerializeField] private FinalScoreWindow _finalScoreWindow;
    [SerializeField] private List<SoundToggleMuter> _soundToggleMuters;
    [SerializeField] private List<LoginButton> _loginButtons;

    public void Initialize(AuthorizationProvider authorizationProvider)
    {
        _finalScoreWindow.Initialize(_playerData);

        foreach (LoginButton loginButton in _loginButtons)
        {
            loginButton.Initialize(authorizationProvider);
        }

        var soundVolumeProvider = new SoundVolumeProvider(_audioMixers, _playerData);

        foreach (SoundToggleMuter soundToggleMuter in _soundToggleMuters)
        {
            soundToggleMuter.Initialize(soundVolumeProvider);
        }
    }
}

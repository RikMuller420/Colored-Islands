using System.Collections;
using System.Collections.Generic;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Effects.Sound
{
	public class BackgroundMusicChanger : MonoBehaviour
	{
	    [SerializeField] private LevelSettings _levelSettings;

	    [SerializeField] private AudioSource _menuMusic;
	    [SerializeField] private AudioSource _gameplayMusic;

	    private LevelChangeEventTracker _levelChangeEventTracker;

		private BackgroundMusicTheme _currentTheme;
		private float _fadeDuration = 3f;
	    private Coroutine _fadeInCoroutine;
		private Coroutine _fadeOutCoroutine;
		private Dictionary<BackgroundMusicTheme, AudioSource> _themeAudios;

	    private void OnEnable()
	    {
	        _levelChangeEventTracker.LevelChanged += OnLevelChanged;
	    }

	    private void OnDisable()
	    {
	        _levelChangeEventTracker.LevelChanged -= OnLevelChanged;
	    }

	    public void Initialize(LevelChangeEventTracker levelChangeEventTracker)
	    {
			_themeAudios = new Dictionary<BackgroundMusicTheme, AudioSource>()
			{
				{ BackgroundMusicTheme.MainMenu, _menuMusic },
				{ BackgroundMusicTheme.Gameplay, _gameplayMusic }
			};

			_levelChangeEventTracker = levelChangeEventTracker;
	        enabled = true;
	    }

	    private void OnLevelChanged(ILevelData levelData)
	    {
	        if (levelData.LevelId == _levelSettings.MainMenuSettings.Id)
	        {
				PlayTheme(BackgroundMusicTheme.MainMenu);
			}
			else
	        {
				PlayTheme(BackgroundMusicTheme.Gameplay);
	        }
	    }

		private void PlayTheme(BackgroundMusicTheme theme)
		{
			if (_currentTheme == theme)
			{
				return;
			}

			TryStopFadeCoroutine();

			AudioSource currentAudio = _themeAudios[_currentTheme];
			AudioSource targetAudio = _themeAudios[theme];
			_fadeOutCoroutine = StartCoroutine(FadeMusic(currentAudio, 0));
			_fadeInCoroutine = StartCoroutine(FadeMusic(targetAudio, 1));
			_currentTheme = theme;
		}

		private void TryStopFadeCoroutine()
	    {
	        if (_fadeOutCoroutine != null)
	        {
	            StopCoroutine(_fadeOutCoroutine);
	        }

			if (_fadeInCoroutine != null)
	        {
	            StopCoroutine(_fadeInCoroutine);
	        }
	    }

		private IEnumerator FadeMusic(AudioSource music, float targetVolume)
		{
			if (music.isPlaying == false && targetVolume != 0)
			{
				music.Play();
			}

			float time = 0f;
			float startVolume = music.volume;

			while (time < _fadeDuration)
			{
				time += Time.deltaTime;
				float normalizedTime = time / _fadeDuration;
				music.volume = Mathf.Lerp(startVolume, targetVolume, normalizedTime);

				yield return null;
			}

			music.volume = targetVolume;

			if (targetVolume == 0)
			{
				music.Stop();
			}
		}
	}
}

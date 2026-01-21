using System.Collections;
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

	    private float _fadeDuration = 3f;
	    private Coroutine _fadeCoroutine;

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
	        _levelChangeEventTracker = levelChangeEventTracker;
	        enabled = true;
	    }

	    private void OnLevelChanged(ILevelData levelData)
	    {
	        if (levelData.LevelId == _levelSettings.MainMenuSettings.Id)
	        {
	            PlayMenuMusic();   
	        }
	        else
	        {
	            PlayGameplayMusic();
	        }
	    }

	    private void PlayMenuMusic()
	    {
	        if (_menuMusic.isPlaying && _menuMusic.volume > 0f)
	        {
	            return;
	        }

	        TryStopFadeCoroutine();
	        _fadeCoroutine = StartCoroutine(FadeMusic(_gameplayMusic, _menuMusic));
	    }

	    private void PlayGameplayMusic()
	    {
	        if (_gameplayMusic.isPlaying && _gameplayMusic.volume > 0f)
	        {
	            return;
	        }

	        TryStopFadeCoroutine();
	        _fadeCoroutine = StartCoroutine(FadeMusic(_menuMusic, _gameplayMusic));
	    }

	    private void TryStopFadeCoroutine()
	    {
	        if (_fadeCoroutine != null)
	        {
	            StopCoroutine(_fadeCoroutine);
	        }
	    }

	    private IEnumerator FadeMusic(AudioSource fadeOutMusic, AudioSource fadeInMusic)
	    {
	        if (fadeInMusic.isPlaying == false)
	        {
	            fadeInMusic.volume = 0f;
	            fadeInMusic.Play();
	        }

	        float time = 0f;
	        float startVolumeFadeOut = fadeOutMusic.volume;
	        float startVolumeFadeIn = fadeInMusic.volume;

	        while (time < _fadeDuration)
	        {
	            time += Time.deltaTime;
	            float normalizedTime = time / _fadeDuration;

	            fadeOutMusic.volume = Mathf.Lerp(startVolumeFadeOut, 0f, normalizedTime);
	            fadeInMusic.volume = Mathf.Lerp(startVolumeFadeIn, 1f, normalizedTime);

	            yield return null;
	        }

	        fadeOutMusic.volume = 0f;
	        fadeInMusic.volume = 1f;
	        fadeOutMusic.Stop();

	        _fadeCoroutine = null;
	    }
	}

}

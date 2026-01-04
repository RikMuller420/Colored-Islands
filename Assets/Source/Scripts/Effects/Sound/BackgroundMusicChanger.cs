using System.Collections;
using UnityEngine;

public class BackgroundMusicChanger : MonoBehaviour
{
    [SerializeField] private AudioSource _menuMusic;
    [SerializeField] private AudioSource _gameplayMusic;

    private LevelLoader _levelLoader;

    private float _fadeDuration = 3f;
    private Coroutine _fadeCoroutine;

    private void OnEnable()
    {
        _levelLoader.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _levelLoader.LevelChanged -= OnLevelChanged;
    }

    public void Initialize(LevelLoader levelLoader)
    {
        _levelLoader = levelLoader;
        enabled = true;
    }

    private void OnLevelChanged()
    {
        if (_levelLoader.CurrentLevelData.Id > 0)
        {
            PlayGameplayMusic();
        }
        else
        {
            PlayMenuMusic();
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

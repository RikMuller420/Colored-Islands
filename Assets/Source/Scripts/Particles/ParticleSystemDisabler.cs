using System.Collections;
using UnityEngine;

public class ParticleSystemDisabler : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private WaitForSeconds _wait;

    void Start()
    {
        _wait = new WaitForSeconds(_particleSystem.main.duration);
        StartCoroutine(DisableAfterPlay());
    }

    private IEnumerator DisableAfterPlay()
    {
        yield return _wait;

        gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (_particleSystem == null)
        {
            if (TryGetComponent(out ParticleSystem system))
            {
                _particleSystem = system;
            }
        }
    }
}

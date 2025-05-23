using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioMixers", menuName = "Custom/AudioMixers")]
public class AudioMixers : ScriptableObject
{
    [SerializeField] private AudioMixerData[] _mixers;

    public IReadOnlyCollection<AudioMixerData> Mixers => _mixers;
}

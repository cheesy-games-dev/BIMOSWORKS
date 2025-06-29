using UnityEngine;
using UnityEngine.Audio;

namespace KadenZombie8.BIMOS.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundPlayer : MonoBehaviour
    {
        protected AudioSource AudioSource;

        protected virtual void Awake() => AudioSource = GetComponent<AudioSource>();

        protected void Play() => AudioSource.Play();

        protected void Play(AudioResource resource)
        {
            AudioSource.resource = resource;
            Play();
        }

        protected void Play(AudioResource resource, float volume)
        {
            AudioSource.resource = resource;
            AudioSource.volume = volume;
            Play();
        }
    }
}

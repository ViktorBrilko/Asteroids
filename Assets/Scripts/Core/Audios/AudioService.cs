using UnityEngine;
using Zenject;

namespace Core.Audios
{
    public class AudioService : MonoBehaviour, IInitializable
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioConfig _config;

        public AudioConfig Config => _config;

        public void Initialize()
        {
            PlayMusic(_config.Music);
        }

        private void PlayMusic(AudioClip musicClip)
        {
            _musicSource.clip = musicClip;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        public void PlaySfx(AudioClip clip, bool isLoop = false)
        {
            if (isLoop) _sfxSource.loop = true;

            _sfxSource.PlayOneShot(clip);
        }

        public void StopSfx()
        {
            _sfxSource.Stop();
        }
    }
}
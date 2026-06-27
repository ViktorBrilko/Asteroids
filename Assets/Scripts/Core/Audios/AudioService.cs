using UnityEngine;
using Zenject;

namespace Core.Audios
{
    public class AudioService : MonoBehaviour, IInitializable
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioConfig _config;

        public void Initialize()
        {
            PlayMusic(_config.Music);
        }
        
        public void StopSfx()
        {
            _sfxSource.Stop();
        }

        public void PlayBulletShot()
        {
            PlaySfx(_config.BulletShoot);
        }
        
        public void PlayLaserShot()
        {
            PlaySfx(_config.LaserShoot);
        }
        
        public void PlayExplosion()
        {
            PlaySfx(_config.Explosion);
        }
        
        public void PlayUiClick()
        {
            PlaySfx(_config.UiClick);
        }
        
        public void PlayCollision()
        {
            PlaySfx(_config.Collision);
        }
        
        public void PlayPlayerDeath()
        {
            PlaySfx(_config.PlayerDeath);
        }

        private void PlayMusic(AudioClip musicClip)
        {
            _musicSource.clip = musicClip;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        private void PlaySfx(AudioClip clip, bool isLoop = false)
        {
            if (isLoop) _sfxSource.loop = true;

            _sfxSource.PlayOneShot(clip);
        }
    }
}
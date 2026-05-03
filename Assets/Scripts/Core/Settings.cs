using Core.Configs;
using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class Settings 
    {
        private const string MIXER_MUSIC = "Music_Volume";
        private const string MIXER_SFX = "SFX_Volume";

        private SettingsConfig _settingsConfig;
        private float _musicVolume;
        private float _sfxVolume;
        private AudioMixer _audioMixer;

        public float MusicVolume => _musicVolume;

        public float SfxVolume => _sfxVolume;
       
        public Settings(SettingsConfig settingsConfig, AudioMixer audioMixer)
        {
            _settingsConfig = settingsConfig;
            _audioMixer = audioMixer;

            _musicVolume = _settingsConfig.DefaultMusicVolume;
            _sfxVolume = _settingsConfig.DefaultSfxVolume;
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _audioMixer.SetFloat(MIXER_MUSIC, _musicVolume);
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _audioMixer.SetFloat(MIXER_SFX, _sfxVolume);
        }
    }
}
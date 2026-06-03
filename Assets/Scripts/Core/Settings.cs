using Core.Configs;
using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class Settings
    {
        private const string MIXER_MUSIC = "Music_Volume";
        private const string MIXER_SFX = "SFX_Volume";
        private readonly AudioMixer _audioMixer;

        private readonly SettingsConfig _settingsConfig;

        public Settings(SettingsConfig settingsConfig, AudioMixer audioMixer)
        {
            _settingsConfig = settingsConfig;
            _audioMixer = audioMixer;

            MusicVolume = _settingsConfig.DefaultMusicVolume;
            SfxVolume = _settingsConfig.DefaultSfxVolume;
        }

        public float MusicVolume { get; private set; }

        public float SfxVolume { get; private set; }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _audioMixer.SetFloat(MIXER_MUSIC, MusicVolume);
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _audioMixer.SetFloat(MIXER_SFX, SfxVolume);
        }
    }
}
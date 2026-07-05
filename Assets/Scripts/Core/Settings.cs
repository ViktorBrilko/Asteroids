using Core.Configs;
using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class Settings
    {
        private const string MusicMixerParameter = "Music_Volume";
        private const string SfxMixerParameter = "SFX_Volume";
        
        private readonly AudioMixer _audioMixer;
        private readonly SettingsConfig _settingsConfig;
        
        public float MusicVolume { get; private set; }

        public float SfxVolume { get; private set; }

        public Settings(SettingsConfig settingsConfig, AudioMixer audioMixer)
        {
            _settingsConfig = settingsConfig;
            _audioMixer = audioMixer;

            MusicVolume = _settingsConfig.DefaultMusicVolume;
            SfxVolume = _settingsConfig.DefaultSfxVolume;
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _audioMixer.SetFloat(MusicMixerParameter, MusicVolume);
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _audioMixer.SetFloat(SfxMixerParameter, SfxVolume);
        }
    }
}
using System;
using Core;
using Core.Audios;
using MVVM;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class MenuPanelViewModel : IInitializable, IDisposable
    {
        public LoadLevelService LoadLevel;

        private AudioService _audioService;
        public WindowsState _windowsState;

        public MenuPanelViewModel(LoadLevelService loadLevel, AudioService audioService, WindowsState windowsState)
        {
            _audioService = audioService;
            LoadLevel = loadLevel;
            _windowsState = windowsState;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        [Method("OnPlayClick")]
        public void OnMenuClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            LoadLevel.LoadLevel();
        }

        [Method("OnSettingsClick")]
        public void OnSettingsClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsSettingsOpen.Value = true;
        }
    }
}
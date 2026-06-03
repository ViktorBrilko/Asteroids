using System;
using Core;
using Core.Audios;
using MVVM;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class GameMenuButtonViewModel : IInitializable, IDisposable
    {
        private readonly AudioService _audioService;
        private readonly WindowsState _windowsState;

        public GameMenuButtonViewModel(AudioService audioService, WindowsState windowsState)
        {
            _audioService = audioService;
            _windowsState = windowsState;
        }

        public void Dispose()
        {
        }

        public void Initialize()
        {
        }

        [Method("OpenMenuButtonClick")]
        public void OnMenuOpenClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsGameMenuOpen.Value = true;
            Time.timeScale = 0;
        }
    }
}
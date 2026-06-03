using MVVM;
using UniRx;

namespace Core
{
    public class WindowsState
    {
        [Data("GameMenuPanelState")] public ReactiveProperty<bool> IsGameMenuOpen = new();
        [Data("MainMenuPanelState")] public ReactiveProperty<bool> IsMainMenuOpen = new(true);
        [Data("SettingPanelState")] public ReactiveProperty<bool> IsSettingsOpen = new();
    }
}
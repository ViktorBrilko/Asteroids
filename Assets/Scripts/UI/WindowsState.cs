using MVVM;
using UniRx;

namespace UI
{
    public class WindowsState
    {
        [Data("SettingPanelState")]
        public ReactiveProperty<bool> IsSettingsOpen = new();
    }
}
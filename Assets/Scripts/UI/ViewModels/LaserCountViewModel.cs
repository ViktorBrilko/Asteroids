using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class LaserCountViewModel : IInitializable, IDisposable
    {
        [Data("LaserCount")] public readonly ReactiveProperty<string> LaserCountText = new();

        public readonly PlayerWeapon Weapon;

        public LaserCountViewModel(PlayerWeapon weapon)
        {
            Weapon = weapon;
        }

        public void Dispose()
        {
            Weapon.OnLaserCountChanged -= OnLaserCountChanged;
        }

        public void Initialize()
        {
            OnLaserCountChanged(Weapon.LaserShootsCount);
            Weapon.OnLaserCountChanged += OnLaserCountChanged;
        }

        private void OnLaserCountChanged(int laserCount)
        {
            LaserCountText.Value = laserCount.ToString();
        }
    }
}
using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class LaserCountViewModel : IInitializable, IDisposable
    {
        public readonly PlayerWeapon Weapon;
        [Data("LaserCount")]
        public readonly ReactiveProperty<string> LaserCountText = new();

        public LaserCountViewModel(PlayerWeapon weapon)
        {
            Weapon = weapon;
        }
        
        public void Initialize()
        {
            OnLaserCountChanged(Weapon.LaserShootsCount);
            Weapon.OnLaserCountChanged += OnLaserCountChanged;
        }

        public void Dispose()
        {
            Weapon.OnLaserCountChanged -= OnLaserCountChanged;
        }
        
        private void OnLaserCountChanged(int laserCount)
        {
            LaserCountText.Value = laserCount.ToString();
        }
    }
}

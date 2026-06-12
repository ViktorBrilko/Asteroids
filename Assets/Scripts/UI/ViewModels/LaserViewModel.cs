using System;
using Cysharp.Threading.Tasks;
using Gameplay.Players;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class LaserViewModel : IInitializable, IDisposable
    {
        [Data("LaserCharge")] public readonly ReactiveProperty<float> LaserChargeImage = new();
        [Data("LaserCount")] public readonly ReactiveProperty<string> LaserCountText = new();

        public readonly PlayerWeapon Weapon;

        public LaserViewModel(PlayerWeapon weapon)
        {
            Weapon = weapon;
        }

        public void Dispose()
        {
            Weapon.OnLaserChargeStarted -= OnLaserChargeStarted;
            Weapon.OnLaserCountChanged -= OnLaserCountChanged;
        }

        public void Initialize()
        {
            LaserChargeImage.Value = 1;
            Weapon.OnLaserChargeStarted += OnLaserChargeStarted;

            OnLaserCountChanged(Weapon.LaserShootsCount);
            Weapon.OnLaserCountChanged += OnLaserCountChanged;
        }

        private async void OnLaserChargeStarted(float laserCooldown)
        {
            var elapsedTime = 0f;

            while (elapsedTime < laserCooldown)
            {
                LaserChargeImage.Value = elapsedTime / laserCooldown;
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        private void OnLaserCountChanged(int laserCount)
        {
            LaserCountText.Value = laserCount.ToString();
        }
    }
}
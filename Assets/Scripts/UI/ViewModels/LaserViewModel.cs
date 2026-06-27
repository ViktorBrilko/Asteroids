using System;
using Cysharp.Threading.Tasks;
using Gameplay.Players.Weapons;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class LaserViewModel : IInitializable, IDisposable
    {
        [Data("LaserCharge")] public readonly ReactiveProperty<float> LaserChargeProgress = new();
        [Data("LaserCount")] public readonly ReactiveProperty<string> LaserCountText = new();
        
        private readonly LaserWeapon _weapon;

        public LaserViewModel(LaserWeapon weapon)
        {
            _weapon = weapon;
        }

        public void Dispose()
        {
            _weapon.OnLaserChargeStarted -= OnLaserChargeStarted;
            _weapon.OnLaserCountChanged -= OnLaserCountChanged;
        }

        public void Initialize()
        {
            LaserChargeProgress.Value = 1;
            _weapon.OnLaserChargeStarted += OnLaserChargeStarted;

            OnLaserCountChanged(_weapon.LaserShootsCount);
            _weapon.OnLaserCountChanged += OnLaserCountChanged;
        }

        private async void OnLaserChargeStarted(float laserCooldown)
        {
            var elapsedTime = 0f;

            while (elapsedTime < laserCooldown)
            {
                LaserChargeProgress.Value = elapsedTime / laserCooldown;
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
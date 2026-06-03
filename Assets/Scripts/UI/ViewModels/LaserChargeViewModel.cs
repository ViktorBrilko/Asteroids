using System;
using Cysharp.Threading.Tasks;
using Gameplay.Players;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class LaserChargeViewModel : IInitializable, IDisposable
    {
        [Data("LaserCharge")] public readonly ReactiveProperty<float> LaserChargeImage = new();

        public readonly PlayerWeapon Weapon;

        public LaserChargeViewModel(PlayerWeapon weapon)
        {
            Weapon = weapon;
        }

        public void Dispose()
        {
            Weapon.OnLaserChargeStarted -= OnLaserChargeStarted;
        }

        public void Initialize()
        {
            LaserChargeImage.Value = 1;
            Weapon.OnLaserChargeStarted += OnLaserChargeStarted;
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
    }
}
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
        public readonly PlayerWeapon Weapon;
        [Data("LaserCharge")] public readonly ReactiveProperty<float> LaserChargeImage = new();

        public LaserChargeViewModel(PlayerWeapon weapon)
        {
            Weapon = weapon;
        }

        public void Initialize()
        {
            LaserChargeImage.Value = 1;
            Weapon.OnLaserChargeStarted += OnLaserChargeStarted;
        }

        public void Dispose()
        {
            Weapon.OnLaserChargeStarted -= OnLaserChargeStarted;
        }

        private async void OnLaserChargeStarted(float laserCooldown)
        {
            float elapsedTime = 0f;

            while (elapsedTime < laserCooldown)
            {
                LaserChargeImage.Value = elapsedTime / laserCooldown;
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }
    }
}
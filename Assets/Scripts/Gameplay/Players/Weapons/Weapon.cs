using Controls;
using Core.Audios;
using Core.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.Players.Weapons
{
    [RequireComponent(typeof(Player))]
    public abstract class Weapon : MonoBehaviour
    {
        protected Player Player;
        protected AudioService AudioService;
        protected WeaponConfig Config;
        protected PlayerActionCommands ActionCommands;
        protected WeaponCoordinator Coordinator;

        [Inject]
        public void Construct(WeaponConfig config, AudioService audioService,
            PlayerActionCommands actionCommands, WeaponCoordinator coordinator)
        {
            Config = config;
            AudioService = audioService;
            ActionCommands = actionCommands;
            Coordinator = coordinator;
        }

        protected abstract UniTask Fire();

        private void Awake()
        {
            Player = GetComponent<Player>();
        }
    }
}
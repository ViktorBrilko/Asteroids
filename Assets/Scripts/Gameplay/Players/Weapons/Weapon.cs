using Controls;
using Core.Audios;
using Core.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Players.Weapons
{
    public abstract class Weapon : MonoBehaviour   
    {
        protected Player Player;
        protected AudioService AudioService;
        protected WeaponConfig Config;
        protected PlayerActionCommands ActionCommands;
        protected bool CanShootBullets = true;
        
        [Inject]
        public void Construct( WeaponConfig config, AudioService audioService,
            PlayerActionCommands actionCommands)
        {
            Config = config;
            AudioService = audioService;
            ActionCommands = actionCommands;
        }
        
        protected abstract void Fire();

        private void Awake()
        {
            Player = GetComponent<Player>();
        }
    }
}
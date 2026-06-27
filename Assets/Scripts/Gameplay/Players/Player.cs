using Core.Configs;
using Gameplay.Base;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    [RequireComponent(typeof(HealthComponent))]
    public class Player : MonoBehaviour
    {
        private PlayerConfig _config;

        public bool IsUncontrollable { get; set; }

        public HealthComponent HealthComponent { get; private set; }
        
        [Inject]
        public void Construct(PlayerConfig config, HealthComponent healthComponent)
        {
            _config = config;
        }
        
        private void Awake()
        {
            HealthComponent = GetComponent<HealthComponent>();
            HealthComponent.Init(_config.Health);
        }
    }
}
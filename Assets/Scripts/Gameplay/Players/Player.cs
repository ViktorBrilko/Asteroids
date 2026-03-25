using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class Player : MonoBehaviour
    {
        private PlayerConfig _config;
        private HealthService _healthService;

        public HealthService HealthService => _healthService;

        [Inject]
        public void Construct(PlayerConfig config)
        {
            transform.SetParent(null);
            _config = config;

            _healthService = GetComponent<HealthService>();
            _healthService.Init(_config.Health);
        }

        public void OnEnable()
        {
            _healthService.OnDied += Die;
        }

        public void OnDisable()
        {
            _healthService.OnDied -= Die;
        }
        
        public void Die()
        {
        }
       
    }
}
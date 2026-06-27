using System.Collections.Generic;
using Core.Configs;
using Gameplay.Enemies;
using Gameplay.Gamefields;
using Gameplay.Players.Weapons;
using Gameplay.Scores;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Base.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _gameFieldPrefab;

        private ConfigProvider _provider;

        [Inject]
        public void Construct(ConfigProvider provider)
        {
            _provider = provider;
        }

        public override void InstallBindings()
        {
            Container.DeclareSignal<ResetSignal<BulletProjectile>>();
            Container.DeclareSignal<ResetSignal<Asteroid>>();
            Container.DeclareSignal<ResetSignal<SmallAsteroid>>();
            Container.DeclareSignal<ResetSignal<Ufo>>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<PlayerCollidedSignal>();

            InstallScore();
            InstallGameField();
        }

        private void InstallScore()
        {
            Dictionary<EnemyType, int> enemyScoreRates = new Dictionary<EnemyType, int>
            {
                { EnemyType.Asteroid, _provider.ScoreConfig.ScoreForAsteroid },
                { EnemyType.SmallAsteroid, _provider.ScoreConfig.ScoreForSmallAsteroid },
                { EnemyType.Ufo, _provider.ScoreConfig.ScoreForUfo }
            };

            Container.Bind<ScoreConfig>().FromInstance(_provider.ScoreConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreLogic>().AsSingle().WithArguments(enemyScoreRates);
        }

        private void InstallGameField()
        {
            Container.Bind<GameFieldConfig>().FromInstance(_provider.GameFieldConfig).AsSingle();
            Container.Bind<GameField>().FromComponentInNewPrefab(_gameFieldPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyGeneratorService>().AsSingle();
        }
    }
}
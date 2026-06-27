using System.IO;
using Newtonsoft.Json;

namespace Core.Configs
{
    public class ConfigProvider
    {
        private const string PlayerConfigName = "player_config.json";
        private const string BulletConfigName = "bullet_config.json";
        private const string GameFieldConfigName = "gamefield_config.json";
        private const string AsteroidConfigName = "asteroid_config.json";
        private const string SmallAsteroidConfigName = "small_asteroid_config.json";
        private const string UfoConfigName = "ufo_config.json";
        private const string ScoreConfigName = "score_config.json";
        private const string WeaponConfigName = "weapon_config.json";
        private const string SettingsConfigName = "settings_config.json";
        private const string AdsConfigName = "ads_config.json";
        private const string CapacitiesConfigName = "scene_capacities_config.json";
        
        public PlayerConfig PlayerConfig { get; private set; }
        public BulletConfig BulletConfig { get; private set; }
        public GameFieldConfig GameFieldConfig { get; private set; }
        public AsteroidConfig AsteroidConfig { get; private set; }
        public SmallAsteroidConfig SmallAsteroidConfig { get; private set; }
        public UfoConfig UfoConfig { get; private set; }
        public ScoreConfig ScoreConfig { get; private set; }
        public WeaponConfig WeaponConfig { get; private set; }
        public SettingsConfig SettingsConfig { get; private set; }
        public AdsConfig AdsConfig { get; private set; }
        public CapacitiesConfig CapacitiesConfig { get; private set; }

        public ConfigProvider()
        {
            BetterStreamingAssets.Initialize();
        }

        public void LoadAll()
        {
            PlayerConfig = LoadFromFile<PlayerConfig>(PlayerConfigName);
            BulletConfig = LoadFromFile<BulletConfig>(BulletConfigName);
            GameFieldConfig = LoadFromFile<GameFieldConfig>(GameFieldConfigName);
            AsteroidConfig = LoadFromFile<AsteroidConfig>(AsteroidConfigName);
            SmallAsteroidConfig = LoadFromFile<SmallAsteroidConfig>(SmallAsteroidConfigName);
            UfoConfig = LoadFromFile<UfoConfig>(UfoConfigName);
            ScoreConfig = LoadFromFile<ScoreConfig>(ScoreConfigName);
            WeaponConfig = LoadFromFile<WeaponConfig>(WeaponConfigName);
            SettingsConfig = LoadFromFile<SettingsConfig>(SettingsConfigName);
            AdsConfig = LoadFromFile<AdsConfig>(AdsConfigName);
            CapacitiesConfig = LoadFromFile<CapacitiesConfig>(CapacitiesConfigName);
        }

        private T LoadFromFile<T>(string fileName)
        {
            if (!BetterStreamingAssets.FileExists(fileName))
            {
                throw new FileNotFoundException($"Failed to load config. File missing.: {fileName}", fileName);
            }

            var json = BetterStreamingAssets.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
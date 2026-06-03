using Newtonsoft.Json;

namespace Core.Configs
{
    public class ConfigProvider
    {
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

        public void LoadAll()
        {
            BetterStreamingAssets.Initialize();

            PlayerConfig = LoadFromFile<PlayerConfig>("player_config.json");
            BulletConfig = LoadFromFile<BulletConfig>("bullet_config.json");
            GameFieldConfig = LoadFromFile<GameFieldConfig>("gamefield_config.json");
            AsteroidConfig = LoadFromFile<AsteroidConfig>("asteroid_config.json");
            SmallAsteroidConfig = LoadFromFile<SmallAsteroidConfig>("small_asteroid_config.json");
            UfoConfig = LoadFromFile<UfoConfig>("ufo_config.json");
            ScoreConfig = LoadFromFile<ScoreConfig>("score_config.json");
            WeaponConfig = LoadFromFile<WeaponConfig>("weapon_config.json");
            SettingsConfig = LoadFromFile<SettingsConfig>("settings_config.json");
            AdsConfig = LoadFromFile<AdsConfig>("ads_config.json");
        }

        private T LoadFromFile<T>(string fileName)
        {
            if (BetterStreamingAssets.FileExists(fileName))
            {
                var json = BetterStreamingAssets.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }
    }
}
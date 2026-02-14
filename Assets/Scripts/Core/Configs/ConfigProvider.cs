using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Configs
{
    public class ConfigProvider
    {
        public PlayerConfig PlayerConfig { get; private set; }
        public BulletConfig BulletConfig { get; private set; }
        public GameFieldConfig GameFieldConfig { get; private set; }
        public AsteroidConfig AsteroidConfig { get; private set; }
        
        public void LoadAll()
        {
            PlayerConfig = LoadFromFile<PlayerConfig>("player_config.json");
            BulletConfig = LoadFromFile<BulletConfig>("bullet_config.json");
            GameFieldConfig = LoadFromFile<GameFieldConfig>("gamefield_config.json");
            AsteroidConfig = LoadFromFile<AsteroidConfig>("asteroid_config.json");
        }
    
        private T LoadFromFile<T>(string fileName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default; 
        }        
    }
}

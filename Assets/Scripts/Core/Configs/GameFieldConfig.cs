namespace Core.Configs
{
    public class GameFieldConfig
    {
        public float XSize { get; set; }
        public float YSize { get; set; }
        public int MaxAsteroids { get; set; }
        public int MaxUfos { get; set; }
        public int AsteroidSpawnCooldown { get; set; }
        public int UfoSpawnCooldown { get; set; }
        public int MaxAttemptsToPlaceEnemy { get; set; }
        public int MinSmallAsteroids { get; set; }
        public int MaxSmallAsteroids { get; set; }
    }
}
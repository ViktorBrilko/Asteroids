namespace Core.Configs
{
    public class BaseEnemyConfig
    {
        public float MoveSpeed { get; set; }
        public float AfterCollisionSpeed { get; set; }
        public int CollisionEffectTime { get; set; }
        public float RotationSpeed { get; set; }
        public int Damage { get; set; }
        public int Health { get; set; }
    }
}
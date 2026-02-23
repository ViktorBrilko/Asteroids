namespace Gameplay.Base
{
    public interface IDamagable
    {
        public void TakeDamage(int damage);
        public void Die();
    }
}
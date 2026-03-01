using Core.Configs;
using Gameplay.Base;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour, IResetable
{
    private SignalBus _signalBus;
    private BulletConfig _bulletConfig;
    private float _currentLifetime;

    [Inject]
    public void Construct(SignalBus signalBus, BulletConfig bulletConfig)
    {
        _signalBus  = signalBus;
        _bulletConfig = bulletConfig;
    }
    
    private void Update()
    {
        Move();
        CheckLifetime();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out HealthService enemy))
        {
            enemy.TakeDamage(_bulletConfig.Damage);
            DestroyBullet();
        }
    }
    
    public void Reset()
    {
        _currentLifetime = 0;
    }
    
    private void CheckLifetime()
    {
        _currentLifetime += Time.deltaTime;
        if (_currentLifetime >= _bulletConfig.MaxLifetime)
        {
            DestroyBullet();
        }
    }
    
    private void Move()
    {
        transform.Translate(Vector3.up * _bulletConfig.Speed * Time.deltaTime,  Space.Self);
    }
    
    private void DestroyBullet()
    {
        _signalBus.Fire(new ResetSignal<Bullet>(this));
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Gameplay.Base
{
    public class ObjectPool<T> : IInitializable where T : Component
    {
        private readonly int _initialCapacity;
        private readonly Core.IFactory<T> _factory;
        private readonly List<T> _pool = new();
        
        public Transform Container { get; }

        public ObjectPool(Core.IFactory<T> factory, Transform container, int initialCapacity)
        {
            _factory = factory;
            _initialCapacity = initialCapacity;
            Container = container;
        }

        public void Initialize()
        {
            for (var i = 0; i < _initialCapacity; i++) AddNewItem();
        }

        public bool TryGetObject(out T result)
        {
            result = _pool.FirstOrDefault(p => p.gameObject.activeSelf == false);
            return result != null;
        }

        public T AddNewItem()
        {
            var spawned = _factory.Create(Container.transform);
            spawned.gameObject.SetActive(false);

            _pool.Add(spawned);
            return spawned;
        }
    }
}
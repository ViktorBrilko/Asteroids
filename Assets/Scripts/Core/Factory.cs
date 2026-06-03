using UnityEngine;
using Zenject;

namespace Core
{
    public class Factory<TProduct> : IFactory<TProduct>
        where TProduct : Component
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;

        public Factory(DiContainer container, GameObject prefab)
        {
            _container = container;
            _prefab = prefab;
        }

        public TProduct Create(Transform parent)
        {
            var product = _container.InstantiatePrefabForComponent<TProduct>(_prefab, parent);

            return product;
        }
    }
}
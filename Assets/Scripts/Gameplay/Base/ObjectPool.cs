using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ObjectPool<T> : IInitializable where T : Component
{
    private readonly int _capacity;
    private readonly Core.IFactory<T> _factory;
    private readonly List<T> _pool = new();

    public ObjectPool(Core.IFactory<T> factory, Transform container, int capacity)
    {
        _factory = factory;
        _capacity = capacity;
        Container = container;
    }

    public Transform Container { get; }

    public void Initialize()
    {
        for (var i = 0; i < _capacity; i++) AddNewItem();
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
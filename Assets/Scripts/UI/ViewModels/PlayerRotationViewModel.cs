using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerRotationViewModel : IInitializable, IDisposable
{
    public readonly PlayerMovement PlayerMovementLogic;
    [Data("Rotation")]
    public readonly  ReactiveProperty<string> _currentRotation = new();

    public PlayerRotationViewModel(PlayerMovement playerMovementLogic)
    {
        PlayerMovementLogic = playerMovementLogic;
    }

    public void Initialize()
    {
        OnRotationChanged();
        PlayerMovementLogic.OnRotationChanged += OnRotationChanged;
    }

    public void Dispose()
    {
        PlayerMovementLogic.OnRotationChanged -= OnRotationChanged;
    }
        
    private void OnRotationChanged(float zRotation = 0)
    {
        _currentRotation.Value = "Rotation " + zRotation.ToString("F1");
    }
}

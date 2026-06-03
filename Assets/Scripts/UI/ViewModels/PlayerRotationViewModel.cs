using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using Zenject;

public class PlayerRotationViewModel : IInitializable, IDisposable
{
    [Data("Rotation")] public readonly ReactiveProperty<string> _currentRotation = new();

    public readonly PlayerMovement PlayerMovementLogic;

    public PlayerRotationViewModel(PlayerMovement playerMovementLogic)
    {
        PlayerMovementLogic = playerMovementLogic;
    }

    public void Dispose()
    {
        PlayerMovementLogic.OnRotationChanged -= OnRotationChanged;
    }

    public void Initialize()
    {
        OnRotationChanged();
        PlayerMovementLogic.OnRotationChanged += OnRotationChanged;
    }

    private void OnRotationChanged(float zRotation = 0)
    {
        _currentRotation.Value = "Rotation " + zRotation.ToString("F1");
    }
}
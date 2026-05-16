using Controls;
using Controls.Joystick;
using MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MobileButtonsView : MonoBehaviour
    {
        [SerializeField] private MobileButton _rotateLeftButton;
        [SerializeField] private MobileButton _rotateRightButton;
        [SerializeField] private Joystick _joystick;
        [Data("FireLaser")] [SerializeField] public Button FireLaserButton;
        [Data("FireBullets")] [SerializeField] public Button FireBulletsButton;

        [Data("RotatingLeft")] public readonly ReactiveProperty<bool> IsRotatingLeft = new();
        [Data("RotatingRight")] public readonly ReactiveProperty<bool> IsRotatingRight = new();
        [Data("MovingState")] public readonly ReactiveProperty<bool> IsMoving = new();
        [Data("XDirection")] public readonly ReactiveProperty<float> XDirection = new();
        [Data("YDirection")] public readonly ReactiveProperty<float> YDirection = new();

        private void OnEnable()
        {
            _rotateLeftButton.OnStateChanged += OnLeftRotation;
            _rotateRightButton.OnStateChanged += OnRightRotation;
            _joystick.OnJoystickPressedDown += OnChangedMovingState;
        }

        private void OnDisable()
        {
            _rotateLeftButton.OnStateChanged -= OnLeftRotation;
            _rotateRightButton.OnStateChanged -= OnRightRotation;
            _joystick.OnJoystickPressedDown -= OnChangedMovingState;
        }

        private void Update()
        {
            XDirection.Value = _joystick.Horizontal;
            YDirection.Value = _joystick.Vertical;
        }

        private void OnLeftRotation(bool isRotating)
        {
            IsRotatingLeft.Value = isRotating;
        }

        private void OnRightRotation(bool isRotating)
        {
            IsRotatingRight.Value = isRotating;
        }

        private void OnChangedMovingState(bool isMoving)
        {
            IsMoving.Value = isMoving;
        }
    }
}
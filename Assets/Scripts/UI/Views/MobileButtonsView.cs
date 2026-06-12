using Controls;
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
        [SerializeField] private MobileButton _moveForwardButton;
        [SerializeField] private MobileButton _moveBackwardButton;
        [SerializeField] public GameObject MobileButtonsPanel;
        [Data("FireLaser")] [SerializeField] public Button FireLaserButton;
        [Data("FireBullets")] [SerializeField] public Button FireBulletsButton;
        [Data("MovingState")] public readonly ReactiveProperty<bool> IsMoving = new();

        [Data("RotatingLeft")] public readonly ReactiveProperty<bool> IsRotatingLeft = new();
        [Data("RotatingRight")] public readonly ReactiveProperty<bool> IsRotatingRight = new();
        [Data("YDirection")] public readonly ReactiveProperty<float> YDirection = new();

        [Setter("MobileButtonsPanelState")]
        public bool MobileButtonsPanelState
        {
            set => MobileButtonsPanel.SetActive(value);
        }

        private void OnEnable()
        {
            _moveForwardButton.OnStateChanged += OnForwardMoving;
            _moveBackwardButton.OnStateChanged += OnBackwardMoving;
            _rotateLeftButton.OnStateChanged += OnLeftRotation;
            _rotateRightButton.OnStateChanged += OnRightRotation;
        }

        private void OnDisable()
        {
            _moveForwardButton.OnStateChanged -= OnForwardMoving;
            _moveBackwardButton.OnStateChanged -= OnBackwardMoving;
            _rotateLeftButton.OnStateChanged -= OnLeftRotation;
            _rotateRightButton.OnStateChanged -= OnRightRotation;
        }

        private void OnLeftRotation(bool isRotating)
        {
            IsRotatingLeft.Value = isRotating;
        }

        private void OnRightRotation(bool isRotating)
        {
            IsRotatingRight.Value = isRotating;
        }

        private void OnForwardMoving(bool isMoving)
        {
            if (isMoving)
                YDirection.Value = 1;
            else
                YDirection.Value = 0;

            IsMoving.Value = isMoving;
        }

        private void OnBackwardMoving(bool isMoving)
        {
            if (isMoving)
                YDirection.Value = -1;
            else
                YDirection.Value = 0;

            IsMoving.Value = isMoving;
        }
    }
}
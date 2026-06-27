using Controls;
using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MobileButtonsView : MonoBehaviour
    {
        [Data("FireLaser")] [SerializeField] public Button FireLaserButton;
        [Data("FireBullets")] [SerializeField] public Button FireBulletsButton;

        [Data("RotatingLeft")] [SerializeField]
        public MobileButton RotateLeftButton;

        [Data("RotatingRight")] [SerializeField]
        public MobileButton RotateRightButton;

        [Data("MoveForward")] [SerializeField]
        public MobileButton MoveForwardButton;

        [Data("MoveBackward")] [SerializeField]
        public MobileButton MoveBackwardButton;

        [SerializeField] public GameObject MobileButtonsPanel;

        [Setter("MobileButtonsPanelState")]
        public bool MobileButtonsPanelState
        {
            set => MobileButtonsPanel.SetActive(value);
        }
    }
}
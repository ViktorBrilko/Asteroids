using MVVM;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class PlayerParametersView : MonoBehaviour
    {
        [Data("Position")] [SerializeField] public TMP_Text PositionText;
        [Data("Rotation")] [SerializeField] public TMP_Text RotationText;
        [Data("Speed")] [SerializeField] public TMP_Text SpeedText;

        [Data("InertionSpeed")] [SerializeField]
        public TMP_Text InertionSpeed;
    }
}
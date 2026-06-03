using MVVM;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class SpeedView : MonoBehaviour
    {
        [Data("Speed")] [SerializeField] public TMP_Text _speedText;
    }
}
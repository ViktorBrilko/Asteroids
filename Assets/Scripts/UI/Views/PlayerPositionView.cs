using MVVM;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class PlayerCoordinatesView : MonoBehaviour
    {
        [Data("Position")] [SerializeField] public TMP_Text _positionText;
    }
}
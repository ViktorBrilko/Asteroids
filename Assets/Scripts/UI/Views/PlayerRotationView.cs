using MVVM;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class PlayerRotationView : MonoBehaviour
    {
        [Data("Rotation")] [SerializeField] public TMP_Text _rotationText;
    }
}
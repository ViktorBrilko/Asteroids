using MVVM;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class InertiaView : MonoBehaviour
    {
        [Data("InertiaSpeed")] [SerializeField] public TMP_Text InertiaSpeedText;
        
    }
}
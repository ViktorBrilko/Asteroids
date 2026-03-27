using System.Collections.Generic;
using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class HealthView : MonoBehaviour
    {
        [Data("Health")]
        [SerializeField] public List<Image> Hearts;
    }
}

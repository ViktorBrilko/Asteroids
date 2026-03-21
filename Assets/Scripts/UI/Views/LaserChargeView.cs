using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class LaserChargeView : MonoBehaviour
    {
        [Data("LaserCharge")]
        [SerializeField] public Image LaserChargeImage;
    }
}
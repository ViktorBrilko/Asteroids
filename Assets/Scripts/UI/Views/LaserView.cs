using MVVM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class LaserView : MonoBehaviour
    {
        [Data("LaserCharge")] [SerializeField] public Image LaserChargeImage;
        [Data("LaserCount")] [SerializeField] public TMP_Text LaserCountText;
    }
}
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class MobileButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public event Action<bool> OnStateChanged;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnStateChanged?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnStateChanged?.Invoke(false);
        }
    }
}
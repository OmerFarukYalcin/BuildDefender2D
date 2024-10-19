using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuilderDefender
{
    // This class handles mouse enter and exit events for UI elements or GameObjects
    // that interact with Unity's Event System.
    public class MouseEnterExitEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Event triggered when the mouse pointer enters the UI element or GameObject
        public event EventHandler OnMouseEnterEvent;

        // Event triggered when the mouse pointer exits the UI element or GameObject
        public event EventHandler OnMouseExitEvent;

        // Called by Unity when the mouse pointer enters the UI element or GameObject
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Invoke the OnMouseEnterEvent event if there are any listeners
            OnMouseEnterEvent?.Invoke(this, EventArgs.Empty);
        }

        // Called by Unity when the mouse pointer exits the UI element or GameObject
        public void OnPointerExit(PointerEventData eventData)
        {
            // Invoke the OnMouseExitEvent event if there are any listeners
            OnMouseExitEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}

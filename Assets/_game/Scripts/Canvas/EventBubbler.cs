using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class EventBubbler : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
        private void Bubble<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> eventFunction) where T : IEventSystemHandler
        {
            var allRaycasted = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, allRaycasted);
            foreach (var raycasted in allRaycasted)
            {
                if (raycasted.gameObject == gameObject)
                {
                    continue;
                }
                ExecuteEvents.Execute(raycasted.gameObject, eventData, eventFunction);
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Bubble(eventData, ExecuteEvents.pointerClickHandler);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Bubble(eventData, ExecuteEvents.pointerDownHandler);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            Bubble(eventData, ExecuteEvents.pointerUpHandler);
        }
    
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyTearrain
{
    public class UIRaycasterHandler : MonoBehaviour, IMoveHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Action<AxisEventData> onMove;
        public Action<PointerEventData> onPointerDown;
        public Action<PointerEventData> onPointerUp;
        public Action<PointerEventData> onPointerEnter;
        public Action<PointerEventData> onPointerExit;

        [SerializeField] private RectTransform rt;


        private void Start()
        {
            if (rt == null)
            {
                rt = GetComponent<RectTransform>();
            }
        }

        public void OnMove(AxisEventData eventData)
        {
            if (onMove != null)
            {
                onMove(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (onPointerDown != null)
            {
                onPointerDown(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (onPointerUp != null)
            {
                onPointerUp(eventData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onPointerEnter != null)
            {
                onPointerEnter(eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (onPointerExit != null)
            {
                onPointerExit(eventData);
            }
        }

        public Vector3 GetPos()
        {
            return transform.position;
        }
        
        public Vector2 GetRectPos()
        {
            return rt.rect.position;
        }
    }
}
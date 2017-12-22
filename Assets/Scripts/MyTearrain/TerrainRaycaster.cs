using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyTearrain
{
    public class TerrainRaycaster : MonoBehaviour
    {
        [SerializeField] private UIRaycasterHandler uiRaycasterHandler;
        [SerializeField] private Camera terrainCamera;
        [SerializeField] private TerrainManager terrainManager;

        private Ray ray;
        private RaycastHit raycastHit;
        private bool hitted;

        private bool pointerEntered;
        private Vector3 oldPointerPos;

        public static TerrainRaycaster instance;

        public Action<Vector3> onPointerDownAction;
        public Action<Vector3> onPointerUpAction;
        public Action<Vector3, Vector3> onPointerMoveAction;
        public bool onDownedPointer;
        
        private Vector3 oldTerrainPointerPos;


        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            ray = new Ray();
            uiRaycasterHandler.onMove += OnMove;
            uiRaycasterHandler.onPointerDown += OnPointerDown;
            uiRaycasterHandler.onPointerUp += OnPointerUp;
            uiRaycasterHandler.onPointerEnter += OnPointerEnter;
            uiRaycasterHandler.onPointerExit += OnPointerExit;
        }

        private void OnMove(AxisEventData axisEventData)
        {
            if (pointerEntered && onDownedPointer && onPointerMoveAction!=null)
            {
                Vector3 moveVector = new Vector3(axisEventData.moveVector.x, 0, axisEventData.moveVector.y);
                onPointerMoveAction(oldTerrainPointerPos, GetRaycastPoint(axisEventData.moveVector));
                oldTerrainPointerPos += moveVector;
            }
        }

        private void OnPointerDown(PointerEventData pointerEventData)
        {
            onDownedPointer = true;
            Vector3 pos = terrainManager.WorldToTerrainPoint(GetRaycastPoint(pointerEventData));
            if (onPointerDownAction != null)
            {
                onPointerDownAction(pos);
            }
        }

        private void OnPointerUp(PointerEventData pointerEventData)
        {
            onDownedPointer = false;
            Vector3 pos = terrainManager.WorldToTerrainPoint(GetRaycastPoint(pointerEventData));
            if (onPointerUpAction != null)
            {
                onPointerUpAction(pos);
            }
        }

        private void OnPointerEnter(PointerEventData pointerEventData)
        {
            pointerEntered = true;
        }

        private void OnPointerExit(PointerEventData pointerEventData)
        {
            pointerEntered = false;
        }

        private void Update()
        {
            Vector3 pos = GetRaycastPoint(Input.mousePosition);
            if (pos.x > 0 && terrainManager != null)
            {
                terrainManager.SetBrushPosTo(new Vector2(raycastHit.point.x, raycastHit.point.z));
                if (onDownedPointer && onPointerMoveAction != null)
                {
                    onPointerMoveAction(oldTerrainPointerPos, pos);
                }
                oldTerrainPointerPos = pos;
            }
        }

        private Vector3 GetRaycastPoint(PointerEventData pointerEventData)
        {
            ray = terrainCamera.ScreenPointToRay(pointerEventData.position);
            Vector3 pos = terrainCamera.ScreenToWorldPoint(uiRaycasterHandler.GetRectPos() - pointerEventData.position);

//            Debug.LogFormat("Point pos: {0}", ray.ToString());
            hitted = Physics.Raycast(ray, out raycastHit, 10000);

            if (hitted)
            {
                return raycastHit.point;
            }

            return Vector3.one * -1;
        }

        private Vector3 GetRaycastPoint(Vector3 pointerPos)
        {
            if (pointerEntered && pointerPos != oldPointerPos)
            {
                oldPointerPos = pointerPos;
                ray = terrainCamera.ScreenPointToRay(Input.mousePosition);
                hitted = Physics.Raycast(ray, out raycastHit, 10000);
            }

            if (hitted)
            {
                return raycastHit.point;
            }

            return Vector3.one * -1;
        }
    }
}
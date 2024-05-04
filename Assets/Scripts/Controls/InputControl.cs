using UnityEngine;
using UnityEngine.EventSystems;


namespace GameGuru.Controls
{
    public class InputControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private LayerMask layerMask;

        private bool isPressed = false;
        private bool isDown = false;
        private bool isUp = false;

        public bool IsPressed => isPressed;
        public bool IsDown => isDown;
        public bool IsUp => isUp;

        public Vector2 Input => lastEventData.position;
        public Vector3 WorldPosition => GetWorldMousePosition();

        private PointerEventData lastEventData;

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            isDown = true;
            lastEventData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
            isUp = true;
            lastEventData = null;
        }

        private void LateUpdate()
        {
            isDown = false;
            isUp = false;
        }
        private Vector3 GetWorldMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(lastEventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                return hit.point;
            }
            return Vector3.zero;
        }
    }
}
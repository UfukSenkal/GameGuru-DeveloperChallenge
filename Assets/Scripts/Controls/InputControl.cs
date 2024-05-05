using UnityEngine;
using UnityEngine.EventSystems;


namespace GameGuru.Controls
{
    public class InputControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private LayerMask layerMask;

        private bool _isPressed = false;
        private bool _isDown = false;
        private bool _isUp = false;

        public bool IsPressed => _isPressed;
        public bool IsDown => _isDown;
        public bool IsUp => _isUp;

        public Vector2 Input => lastEventData.position;
        public Vector3 WorldPosition => GetWorldMousePosition();

        private PointerEventData lastEventData;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _isDown = true;
            lastEventData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            _isUp = true;
            lastEventData = null;
        }

        private void LateUpdate()
        {
            _isDown = false;
            _isUp = false;
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
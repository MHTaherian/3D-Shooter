using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes.Game.UI.InGameUI.InputManager
{
    public class Joystick : InputManager, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _stickRectTransform;
        [SerializeField] private RectTransform _backgroundRectTransform;
        [SerializeField] private RectTransform _centerRectTransform;

        public void OnDrag(PointerEventData eventData)
        {
            // We want the stick to remain in a the background circle and not to overlap it
            var touchPos = eventData.position;
            Vector2 centerPos = _backgroundRectTransform.position;
            var backgroundHalfWidth = _backgroundRectTransform.sizeDelta.x / 2;
            var stickClampedOffset =
                Vector2.ClampMagnitude(touchPos - centerPos, backgroundHalfWidth);

            _stickRectTransform.position = centerPos + stickClampedOffset;
            var inputValue = stickClampedOffset / backgroundHalfWidth;
             OnInputValueUpdated?.Invoke(inputValue);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _backgroundRectTransform.position = eventData.position;
            _stickRectTransform.position = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _backgroundRectTransform.position = _centerRectTransform.position;
            _stickRectTransform.position = _backgroundRectTransform.position;
            OnInputValueUpdated?.Invoke(Vector2.zero);
        }

        public override event Action<Vector2> OnInputValueUpdated;
    }
}
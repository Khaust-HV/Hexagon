using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemActions {
    public sealed class TouchscreenInputActions {
        private readonly InputMap _inputMap;
        private Vector2 _firstTouchPosition;

        public event Action<Vector2> TapPosition;
        public event Action<bool> FirstTouchActive;
        public event Action<Vector2> SingleSwipeDelta;
        public event Action<bool> SecondTouchActive;
        public event Action<Vector2, Vector2> DoubleTouchPositions;

        public TouchscreenInputActions(InputMap inputMap) {
            _inputMap = inputMap;
        }

        #region GameplayInput
            public void SetGameplayInputActive(bool isActive) {
                if (isActive) {
                    _inputMap.GameplayInput.Enable();

                    _inputMap.GameplayInput.TapOnScreenPress.canceled += _ => TapOnScreen();
                    _inputMap.GameplayInput.FirstTouchContact.started += _ => FirstTouchOnEnable();
                    _inputMap.GameplayInput.FirstTouchContact.canceled += _ => FirstTouchOnDisable();
                    _inputMap.GameplayInput.FirstTouchPosition.performed += FirstTouchPosition;
                    _inputMap.GameplayInput.SingleSwipeOnScreen.performed += SingleSwipe;

                    _inputMap.GameplayInput.SecondTouchContact.started += _ => SecondTouchOnEnable();
                    _inputMap.GameplayInput.SecondTouchContact.canceled += _ => SecondTouchOnDisable();
                    _inputMap.GameplayInput.SecondTouchPosition.performed += DoubleTouch;
                }
                else {
                    _inputMap.GameplayInput.Disable();

                    _inputMap.GameplayInput.TapOnScreenPress.canceled -= _ => TapOnScreen();
                    _inputMap.GameplayInput.FirstTouchContact.started -= _ => FirstTouchOnEnable();
                    _inputMap.GameplayInput.FirstTouchContact.canceled -= _ => FirstTouchOnDisable();
                    _inputMap.GameplayInput.FirstTouchPosition.performed -= FirstTouchPosition;
                    _inputMap.GameplayInput.SingleSwipeOnScreen.performed -= SingleSwipe;

                    _inputMap.GameplayInput.SecondTouchContact.started -= _ => SecondTouchOnEnable();
                    _inputMap.GameplayInput.SecondTouchContact.canceled -= _ => SecondTouchOnDisable();
                    _inputMap.GameplayInput.SecondTouchPosition.performed -= DoubleTouch;
                }
            }

            private void TapOnScreen() {
                TapPosition?.Invoke(_inputMap.GameplayInput.TapOnScreenPosition.ReadValue<Vector2>());
            }

            private void FirstTouchOnEnable() {
                FirstTouchActive?.Invoke(true);
            }
            private void FirstTouchOnDisable() {
                FirstTouchActive?.Invoke(false);
            }

            private void FirstTouchPosition(InputAction.CallbackContext context) {
                _firstTouchPosition = context.ReadValue<Vector2>();
            }

            private void SingleSwipe(InputAction.CallbackContext context) {
                SingleSwipeDelta?.Invoke(context.ReadValue<Vector2>());
            }

            private void SecondTouchOnEnable() {
                SecondTouchActive?.Invoke(true);
            }
            private void SecondTouchOnDisable() {
                SecondTouchActive?.Invoke(false);
            }

            private void DoubleTouch(InputAction.CallbackContext context) {
                DoubleTouchPositions?.Invoke(_firstTouchPosition, context.ReadValue<Vector2>());
            }
        #endregion
    }
}
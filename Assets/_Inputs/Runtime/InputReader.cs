using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem.Input {
    public enum InputDeviceType {
        KeyboardMouse,
        Gamepad
    }

    public class InputReader : MonoBehaviour {
        public event Action NextPressed;
        public event Action SkipPressed;

        public event Action<InputDeviceType> OnInputDeviceChanged;

        private DialogueInputActions inputActionsAsset;

        private InputDeviceType currentDevice = InputDeviceType.KeyboardMouse;
        public InputDeviceType CurrentDevice => currentDevice;

        private void Awake() {
            inputActionsAsset = new DialogueInputActions();
            inputActionsAsset.Enable();
        }

        private void OnEnable() {
            if (inputActionsAsset == null) {
                Debug.LogError($"{nameof(inputActionsAsset)} is null");
                return;
            }

            inputActionsAsset.Enable();

            inputActionsAsset.Dialogue.Next.performed += OnNextPerformed;
            inputActionsAsset.Dialogue.Skip.performed += OnSkipPerformed;
        }

        private void OnDisable() {
            if (inputActionsAsset == null) { return; }

            inputActionsAsset.Dialogue.Next.performed -= OnNextPerformed;
            inputActionsAsset.Dialogue.Skip.performed -= OnSkipPerformed;

            inputActionsAsset.Disable();
        }

        private void OnNextPerformed(InputAction.CallbackContext context) {
            TrackDevice(context);
            NextPressed?.Invoke();
        }

        private void OnSkipPerformed(InputAction.CallbackContext context) {
            TrackDevice(context);
            SkipPressed?.Invoke();
        }

        private void TrackDevice(InputAction.CallbackContext context) {
            InputDevice device = context.control.device;

            InputDeviceType newType = device is Gamepad ? InputDeviceType.Gamepad : InputDeviceType.KeyboardMouse;

            if (newType != currentDevice) {
                currentDevice = newType;
                OnInputDeviceChanged?.Invoke(currentDevice);
            }
        }


        [ContextMenu("Simulate Gamepad Input")]
        public void SimulateGamepad() {
            InputDeviceType oldDevice = currentDevice;
            currentDevice = InputDeviceType.Gamepad;
            if (oldDevice != currentDevice) { OnInputDeviceChanged?.Invoke(currentDevice); }
        }

        [ContextMenu("Simulate Keyboard Input")]
        public void SimulateKeyboard() {
            InputDeviceType oldDevice = currentDevice;
            currentDevice = InputDeviceType.KeyboardMouse;
            if (oldDevice != currentDevice) { OnInputDeviceChanged?.Invoke(currentDevice); }
        }

    }
}
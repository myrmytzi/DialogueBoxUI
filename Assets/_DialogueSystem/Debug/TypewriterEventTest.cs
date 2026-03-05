using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem.UI {
    public class TypewriterEventTest : MonoBehaviour {
        [SerializeField] private TypewriterText typewriter;

        private int finishCount;

        private void Awake() {
            typewriter.OnTypingFinished += HandleFinished;
        }

        private void Start() {
            typewriter.Play("Testing event firing.");
        }

        private void Update() {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) {
                typewriter.CompleteInstantly();
            }
        }

        private void HandleFinished() {
            finishCount++;
            Debug.Log($"Typing finished. Count: {finishCount}");
        }
    }
}
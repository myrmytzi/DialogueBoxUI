using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem.UI {
    public class TypewriterInterruptTest : MonoBehaviour {
        [SerializeField] private TypewriterText typewriter;

        private void Start() {
            typewriter.Play("Press [interact] to begin.");
        }

        private void Update() {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) {
                typewriter.CompleteInstantly();
            }
        }
    }
}
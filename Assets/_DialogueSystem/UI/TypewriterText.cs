using TMPro;
using UnityEngine;
using System;
using System.Collections;

namespace DialogueSystem.UI {
    [RequireComponent(typeof(TMP_Text))]
    public sealed class TypewriterText : MonoBehaviour {
        [SerializeField] private float charactersPerSecond = 40f;

        private TMP_Text textComponent;
        private Coroutine typingRoutine;
        private bool isTyping;

        public bool IsTyping => isTyping;

        public event Action OnTypingFinished;

        private void Awake() {
            textComponent = GetComponent<TMP_Text>();
        }

        public void Play(string text) {
            if (typingRoutine != null) { StopCoroutine(typingRoutine); }

            textComponent.text = text;
            textComponent.ForceMeshUpdate();

            textComponent.maxVisibleCharacters = 0;

            typingRoutine = StartCoroutine(TypeRoutine());
            // Debug.Log("Starting new typewriter line");
        }

        public void CompleteInstantly() {
            if (!isTyping) { return; }

            if (typingRoutine != null) { StopCoroutine(typingRoutine); }

            textComponent.maxVisibleCharacters = textComponent.textInfo.characterCount;

            isTyping = false;
            OnTypingFinished?.Invoke();
        }

        private IEnumerator TypeRoutine() {
            isTyping = true;

            int totalCharacters = textComponent.textInfo.characterCount;

            float delay = 1f / charactersPerSecond;

            for (int i = 0; i < totalCharacters; i++) {
                textComponent.maxVisibleCharacters = i;
                yield return new WaitForSeconds(delay);
            }
            textComponent.maxVisibleCharacters = totalCharacters;
            
            isTyping = false;
            typingRoutine = null;
            
            // Debug.Log("Typing finished");
            OnTypingFinished?.Invoke();
        }
    }
}
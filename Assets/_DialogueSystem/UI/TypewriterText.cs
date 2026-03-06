using TMPro;
using UnityEngine;
using System;
using System.Collections;

namespace DialogueSystem.UI {
    [RequireComponent(typeof(TMP_Text))]
    public sealed class TypewriterText : MonoBehaviour {
        [SerializeField] private float charactersPerSecond = 40f;

        private TMP_Text text;
        private Coroutine typingRoutine;
        private bool isTyping;

        public bool IsTyping => isTyping;
        public int VisibleCharacterCount => text.maxVisibleCharacters;

        public event Action OnTypingFinished;

        private void Awake() {
            text = GetComponent<TMP_Text>();
        }

        public void StartTyping(string text) {
            if (typingRoutine != null) { StopCoroutine(typingRoutine); }

            this.text.text = text;
            this.text.ForceMeshUpdate();

            this.text.maxVisibleCharacters = 0;

            typingRoutine = StartCoroutine(TypeRoutine());
        }

        public void CompleteInstantly() {
            if (!isTyping) { return; }

            if (typingRoutine != null) { StopCoroutine(typingRoutine); }

            text.maxVisibleCharacters = text.textInfo.characterCount;

            isTyping = false;
            OnTypingFinished?.Invoke();
        }

        public void SetVisibleCharacters(int count) {
            text.maxVisibleCharacters = count;
        }

        public void SetTextInstant(string value) {
            text.text = value;
        }

        private IEnumerator TypeRoutine() {
            isTyping = true;

            int totalCharacters = text.textInfo.characterCount;

            float delay = 1f / charactersPerSecond;

            for (int i = 0; i < totalCharacters; i++) {
                text.maxVisibleCharacters = i;
                yield return new WaitForSeconds(delay);
            }
            text.maxVisibleCharacters = totalCharacters;
            
            isTyping = false;
            typingRoutine = null;
            
            // Debug.Log("Typing finished");
            OnTypingFinished?.Invoke();
        }
    }
}
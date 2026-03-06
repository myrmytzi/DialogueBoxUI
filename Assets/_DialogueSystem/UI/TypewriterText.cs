using TMPro;
using UnityEngine;
using System;
using System.Collections;

namespace DialogueSystem.UI {
    [RequireComponent(typeof(TMP_Text))]
    public sealed class TypewriterText : MonoBehaviour {
        [Header("References")]
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip typingClip;

        [Header("Settings")]
        [SerializeField] private float charactersPerSecond = 40f;

        [Header("Typing Sound")]
        [SerializeField] private bool enableTypingSound = true;

        [SerializeField, Tooltip("Play per word if false")]
        private bool playPerCharacter = true;

        [SerializeField] private int charactersPerTick = 1;

        [Header("Punctuation Pause")]
        [SerializeField] private bool enablePunctuationPause = true;

        [SerializeField] private float commaPause = 0.15f;
        [SerializeField] private float periodPause = 0.35f;

        private TMP_Text text;
        private Coroutine typingRoutine;

        public bool IsTyping { get; private set; }

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
            if (!IsTyping) { return; }

            if (typingRoutine != null) { StopCoroutine(typingRoutine); }

            text.maxVisibleCharacters = text.textInfo.characterCount;

            IsTyping = false;
            OnTypingFinished?.Invoke();
        }

        public void SetVisibleCharacters(int count) {
            text.maxVisibleCharacters = count;
        }

        public void SetTextInstant(string value) {
            text.text = value;
        }

        private IEnumerator TypeRoutine() {
            IsTyping = true;

            int totalCharacters = text.textInfo.characterCount;

            float baseDelay = 1f / charactersPerSecond;

            for (int i = 0; i < totalCharacters; i++) {
                text.maxVisibleCharacters = i + 1;
                char c = text.textInfo.characterInfo[i].character;

                float delay = baseDelay + GetPunctuationDelay(c);
                PlayTypingSound(i, c);
                yield return new WaitForSeconds(delay);
            }
            text.maxVisibleCharacters = totalCharacters;

            IsTyping = false;
            typingRoutine = null;

            OnTypingFinished?.Invoke();
        }

        private static bool IsWordBoundary(char c) {
            return char.IsWhiteSpace(c);
        }

        private float GetPunctuationDelay(char c) {
            if (!enablePunctuationPause) { return 0f; }
            return c switch {
                ',' => commaPause,
                '.' or '!' or '?' => periodPause,
                _ => 0f
            };
        }

        private bool IsPunctuation(char c) {
            return c switch {
                ',' or '.' or '!' or '?' => true,
                _ => false
            };
        }

        private void PlayTypingSound(int index, char currentChar) {
            if (!enableTypingSound || audioSource == null || typingClip == null) { return; }
            if (playPerCharacter) {
                if (index % charactersPerTick == 0
                    && !char.IsWhiteSpace(currentChar)
                    && !IsPunctuation(currentChar)) { audioSource.PlayOneShot(typingClip); }
            }
            else {
                if (IsWordBoundary(currentChar)) { audioSource.PlayOneShot(typingClip); }
            }
        }
    }
}
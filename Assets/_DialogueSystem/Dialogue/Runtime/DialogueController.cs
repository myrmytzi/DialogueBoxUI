using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem.Dialogue.Data;
using DialogueSystem.UI;
using InputSystem.Runtime;

namespace DialogueSystem.Dialogue.Runtime {
    public class DialogueController : MonoBehaviour {
        [Header("Data")]
        [SerializeField] private DialogueSequence sequence;

        [SerializeField] private InputReader inputReader;

        [Header("UI References")]
        [SerializeField] private GameObject dialoguePanel;

        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private Image portraitImage;

        [SerializeField] private TMP_Text dialogueText;

        private TypewriterText typewriter;

        private int currentIndex;

        private DialogueEntry CurrentEntry => sequence.Entries[currentIndex];

        private void Awake() {
            typewriter = dialogueText.GetComponent<TypewriterText>();
            if (!dialoguePanel.activeInHierarchy) { dialoguePanel.SetActive(true); }
        }

        private void OnEnable() {
            inputReader.NextPressed += HandleNext;
            inputReader.SkipPressed += HandleSkip;
            inputReader.OnInputDeviceChanged += HandleDeviceChanged;
        }
        
        private void OnDisable() {
            inputReader.NextPressed -= HandleNext;
            inputReader.SkipPressed -= HandleSkip;
            inputReader.OnInputDeviceChanged -= HandleDeviceChanged;
            
        }

        private void Start() {
            StartSequence();
        }

        private void StartSequence() {
            currentIndex = 0;
            DisplayCurrentEntry();
        }

        private void DisplayCurrentEntry() {
            DialogueEntry entry = CurrentEntry;

            speakerNameText.text = entry.SpeakerName;
            portraitImage.sprite = entry.Portrait;

            typewriter.Play(entry.Text);
        }

        public void HandleNext() {
            if (typewriter.IsTyping) {
                typewriter.CompleteInstantly();
                return;
            }
            AdvanceDialogue();
        }

        public void HandleSkip() {
            EndDialogue();
        }

        private void AdvanceDialogue() {
            currentIndex++;

            if (currentIndex >= sequence.Entries.Count) {
                EndDialogue();
                return;
            }

            DisplayCurrentEntry();
        }

        private void EndDialogue() {
            dialoguePanel.SetActive(false);
            gameObject.SetActive(false);
        }

        private void HandleDeviceChanged(InputDeviceType newDevice) {
            // update glyphs
        }
    }
}
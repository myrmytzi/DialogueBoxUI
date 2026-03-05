using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem.Dialogue.Data;
using DialogueSystem.UI;

namespace DialogueSystem.Dialogue.Runtime {
    public class DialogueController : MonoBehaviour {
        [Header("Data")]
        [SerializeField] private DialogueSequence sequence;

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
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem.Dialogue.Data;
using DialogueSystem.UI;
using DialogueSystem.Input;

namespace DialogueSystem.Dialogue.Runtime {
    public class DialogueController : MonoBehaviour {
        [Header("Data")]
        [SerializeField] private DialogueSequence sequence;

        [SerializeField] private InputReader inputReader;
        [SerializeField] private GlyphMapping mapping;
        
        [SerializeField] private string nextLabelTemplate = "[next]Next";
        [SerializeField] private string skipLabelTemplate = "[skip]Skip";

        [Header("UI References")]
        [Space(10)]
        [SerializeField] private GameObject dialoguePanel;
        
        [Space(10)]
        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private Image portraitImage;
        [SerializeField] private TMP_Text dialogueText;
        
        [Space(10)]
        [SerializeField] private TMP_Text nextButtonLabel;
        [SerializeField] private TMP_Text skipButtonLabel;

        private TypewriterText typewriter;

        private int currentIndex;
        private string currentRawText;
        private string currentProcessedText;
        private GlyphProcessor glyphProcessor;

        private DialogueEntry CurrentEntry => sequence.Entries[currentIndex];

        private void Awake() {
            typewriter = dialogueText.GetComponent<TypewriterText>();
            glyphProcessor = new GlyphProcessor(mapping);
            UpdateButtonLabels(inputReader.CurrentDevice);
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
            dialoguePanel.SetActive(false);
        }

        public void StartSequence() {
            dialoguePanel.SetActive(true);
            currentIndex = 0;
            currentRawText = null;
            currentProcessedText = null;
            DisplayCurrentEntry();
        }

        private void DisplayCurrentEntry() {
            DialogueEntry entry = CurrentEntry;

            speakerNameText.text = entry.SpeakerName;
            portraitImage.sprite = entry.Portrait;

            currentRawText = entry.Text;

            currentProcessedText = glyphProcessor.Process(currentRawText, inputReader.CurrentDevice);

            typewriter.StartTyping(currentProcessedText);
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
        }

        private void HandleDeviceChanged(InputDeviceType device) {
            if (string.IsNullOrEmpty(currentRawText)) { return; }

            int visibleChars = typewriter.VisibleCharacterCount;

            currentProcessedText = glyphProcessor.Process(currentRawText, device);

            typewriter.SetTextInstant(currentProcessedText);
            typewriter.SetVisibleCharacters(visibleChars);
            
            UpdateButtonLabels(device);
        }

        private void UpdateButtonLabels(InputDeviceType device) {
            nextButtonLabel.text = glyphProcessor.Process(nextLabelTemplate, device);
            skipButtonLabel.text = glyphProcessor.Process(skipLabelTemplate, device);
        }
    }
}
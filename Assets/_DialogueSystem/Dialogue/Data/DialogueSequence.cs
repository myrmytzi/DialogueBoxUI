using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Dialogue.Data {
    [CreateAssetMenu(
        fileName = "DialogueSequence",
        menuName = "Dialogue System/Dialogue Sequence")]
    public sealed class DialogueSequence : ScriptableObject {
        [SerializeField] private List<DialogueEntry> dialogueEntries = new();

        public IReadOnlyList<DialogueEntry> DialogueEntries => dialogueEntries;
        public int Count => dialogueEntries?.Count ?? 0;

        public DialogueEntry GetDialogueEntry(int index) {
            if (dialogueEntries == null || index < 0 || index >= dialogueEntries.Count)
                return null;
            return dialogueEntries[index];
        }
    }
}
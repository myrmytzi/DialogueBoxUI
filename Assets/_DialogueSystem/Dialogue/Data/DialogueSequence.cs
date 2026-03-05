using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogueSystem.Dialogue.Data {
    [CreateAssetMenu(
        fileName = "DialogueSequence",
        menuName = "Dialogue System/Dialogue Sequence")]
    public sealed class DialogueSequence : ScriptableObject {
        [FormerlySerializedAs("dialogueEntries")] [SerializeField] private List<DialogueEntry> entries = new();

        public IReadOnlyList<DialogueEntry> Entries => entries;
        public int Count => entries?.Count ?? 0;

        public DialogueEntry GetDialogueEntry(int index) {
            if (entries == null || index < 0 || index >= entries.Count)
                return null;
            return entries[index];
        }
    }
}
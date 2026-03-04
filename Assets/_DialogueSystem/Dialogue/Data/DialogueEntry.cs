using System;
using UnityEngine;

namespace DialogueSystem.Dialogue.Data {
    [Serializable]
    public class DialogueEntry {
        [Header("Speaker")]
        [SerializeField] private string speakerName;
        [SerializeField] private Sprite portrait;

        [Header("Dialogue")]
        [TextArea(3, 10)] [SerializeField] private string text;

        public string SpeakerName => speakerName;
        public Sprite Portrait => portrait;
        public string Text => text;
    }
}
using DialogueSystem.Input;
using UnityEngine;

namespace DialogueSystem.UI {
    [CreateAssetMenu(
        fileName = "GlyphMapping",
        menuName = "Dialogue System/Glyph Mapping")]
    public class GlyphMapping : ScriptableObject {
        [System.Serializable]
        public class GlyphEntry {
            public string TokenName; // "interact", "next", "skip" etc
            public string SpriteNameKeyboard;
            public string SpriteNameGamepad;  
        }
        
        public GlyphEntry[] Entries;

        public string GetSpriteName(string token, Input.InputDeviceType device) {
            for (int index = 0; index < Entries.Length; index++) {
                if (Entries[index].TokenName == token) {
                    return device == InputDeviceType.Gamepad 
                        ? Entries[index].SpriteNameGamepad 
                        : Entries[index].SpriteNameKeyboard;
                }
            }
            Debug.LogWarning($"Glyph token '{token}' not found ");
            return null;
        }
        
    }
    
}
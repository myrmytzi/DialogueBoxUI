using System;
using UnityEngine;

namespace DialogueSystem.UI {
    public class TypewriterCancelTest : MonoBehaviour {
        [SerializeField] private TypewriterText typewriter;

        private void Start() {
            StartCoroutine(TestRoutine());
        }
        
        private System.Collections.IEnumerator TestRoutine() {
            typewriter.StartTyping("This line should be interrupted halfway.");
            yield return new WaitForSeconds(1f);
            
            typewriter.StartTyping("Second line should start cleanly.");
            yield return new WaitForSeconds(1f);
            
            typewriter.StartTyping("Third line replaces second.");
        }
    }
}
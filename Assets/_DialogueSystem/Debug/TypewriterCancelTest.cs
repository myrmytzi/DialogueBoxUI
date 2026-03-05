using System;
using UnityEngine;

namespace DialogueSystem.UI {
    public class TypewriterCancelTest : MonoBehaviour {
        [SerializeField] private TypewriterText typewriter;

        private void Start() {
            StartCoroutine(TestRoutine());
        }
        
        private System.Collections.IEnumerator TestRoutine() {
            typewriter.Play("This line should be interrupted halfway.");
            yield return new WaitForSeconds(1f);
            
            typewriter.Play("Second line should start cleanly.");
            yield return new WaitForSeconds(1f);
            
            typewriter.Play("Third line replaces second.");
        }
    }
}
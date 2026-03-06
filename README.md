# Dialogue System Demo
A small dialogue system prototype built in Unity demonstrating:
- Typewriter text
- Input glyph substitution (keyboard / gamepad)
- Runtime device switching
- Dialogue sequences driven by ScriptableObjects

## Unity Version
Unity 6000.3.10f1

## How to Run the Demo
### Option 1 - Run the Build
- Open the Builds/Windows folder
- Launch: `DialogueBoxUI.exe`
- Press `Play` to start the dialogue sequence.
### Option 2 - Run in Unity
- Open the project in Unity.
- Load the scene: `Assets/Scenes/Demo.unity`
- Press `Play` in the editor to start the scene.
- Press `Play`in the gameview to start the sequence.

## System Overview
The system is organized into several main components.

### Dialogue Data
Dialogue is authored using ScriptableObjects.
```
DialogueSequence
    └─ DialogueEntry
            SpeakerName
            Portrait
            Text
```
This keeps dialogue content separate from UI logic.

### Dialogue Controller
`DialogueController` manages:
- dialogue progression
- input handling
- device switching
- glyph processing
- UI updates

Responsibilities include:
- displaying entries
- coordinating the typewriter effect
- responding to input events
- reprocessing glyphs when the input device changes

### Typewriter System
`TypewriterText` handles:
- character-by-character text reveal
- configurable typing speed
- optional typing sound
- punctuation pauses
- instant completion when skipping

It uses `TMP_Text.maxVisibleCharacters` to reveal text efficiently without rebuilding the string.

### Glyph System
The glyph system allows dialogue text to contain input tokens.

Example:
```
Press [next] to continue.
```
At runtime these tokens are replaced with TextMeshPro sprite tags:
```
Press <sprite name="Key_Space"> to continue.
```
Replacement is performed using Regex to safely detect tokens.
When the input device changes:
1. Dialogue is reprocessed
2. Text is reassigned
3. Visible character count is restored
4. The typewriter coroutine continues uninterrupted

This avoids restarting the typing animation.

## Known Limitations / Future Improvements
### Button labels
Currently button labels use token templates assignable in the inspector. A better solution would be storing UI labels in a ScriptableObject to support:
- reuse across multiple dialogue UIs
- easier localization
- centralized editing

### Portrait animation
`DialogueEntry` currently references a Sprite. A more flexible approach would be referencing a Prefab GameObject or "Speaker" ScriptableObject, allowing:
- spritesheet animation support (idle, different expressions, etc)
- character-specific UI elements

### Typewriter Configuration
`TypewriterText` settings are currently serialized directly on the component. With more time this would be moved to a ScriptableObject configuration, allowing:
- consistent settings across multiple dialogue views
- easier tuning
- reusable presets

## Design Decisions
### ScriptableObject Data Model
Dialogue data is separated from runtime systems using ScriptableObjects.<br>
Benefits:
- easy authoring
- reusable dialogue sequences
- clear separation of content vs logic

### Regex-Based Token Parsing
Glyph tokens are processed using Regex rather than string replacement to avoid:
- accidental partial matches
- replacing text inside TMP tags
- incorrect token parsing

### Runtime Device switching
The system updates glyphs without restarting the typewriter effect.<br>
This required storing:
- raw dialogue text
- processed dialogue text
- visible character counts

When the device changes, the system:
1. reprocesses glyph tokens
2. updates the TMP text
3. restores visible characters

This allows seamless switching between keyboard and gamepad

## Notes
This prototype focuses on system architecture and extensibility rather than a full production dialogue solution.


## Third Party Attributions
- Character, Environment & UI Sprites by Crusenho at https://crusenho.itch.io/ 
<br>Licensed under Creative Commons Attribution 4.0 International (CC BY 4.0)
https://creativecommons.org/licenses/by/4.0/

- Glyphs:
	- <a href="https://www.flaticon.com/free-icons/keyboard-key" title="keyboard key icons">Keyboard key icons created by littleicon - Flaticon</a>
	- <a href="https://www.flaticon.com/free-icons/keyboard-key" title="keyboard-key icons">Keyboard-key icons created by littleicon - Flaticon</a>
	- <a href="https://www.flaticon.com/free-icons/joystick" title="joystick icons">Joystick icons created by tulpahn - Flaticon</a>

- SFX by Jason Steele at https://filmcow.itch.io/filmcow-sfx
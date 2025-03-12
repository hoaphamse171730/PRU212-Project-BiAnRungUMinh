using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string speaker; // Name of the speaker (NPC, main character, etc.)
    [TextArea(3, 10)]
    public string[] sentences; // Multiple dialogue lines for the conversation
    [TextArea(3, 10)]
    public string note;
}

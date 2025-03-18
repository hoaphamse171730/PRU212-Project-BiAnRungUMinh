using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private string speaker;
    [TextArea(3, 10)]
    [SerializeField] private string[] sentences;
    [TextArea(3, 10)]
    [SerializeField] private string note;
    [SerializeField] private DialogueChoice[] choices; // New field for decision options.

    public string Speaker => speaker;
    public string[] Sentences => sentences;
    public string Note => note;
    public DialogueChoice[] Choices => choices;
}

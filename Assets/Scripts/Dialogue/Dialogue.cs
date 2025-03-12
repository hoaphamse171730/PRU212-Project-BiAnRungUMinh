using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private string speaker;
    [TextArea(3, 10)]
    [SerializeField] private string[] sentences;
    [TextArea(3, 10)]
    [SerializeField] private string note;

    public string Speaker => speaker;
    public string[] Sentences => sentences;
    public string Note => note;

}

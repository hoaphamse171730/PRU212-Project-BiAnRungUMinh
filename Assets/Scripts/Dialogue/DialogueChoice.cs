using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    [SerializeField] private string choiceText;
    [SerializeField] private Dialogue nextDialogue; // Optional branch to another dialogue.
    [SerializeField] private string eventID; // Optional event identifier to trigger endings or other events.

    public string ChoiceText => choiceText;
    public Dialogue NextDialogue => nextDialogue;
    public string EventID => eventID;
}

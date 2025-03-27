using UnityEngine;

public class NPCSwitcher : MonoBehaviour
{
    [Header("Assign your NPC GameObjects")]
    public GameObject npcA; 
    public GameObject npcB; 

    private void Start()
    {
        string decision = DecisionManager.SelectedEventID;

        if (decision == "ShowGoodNPC")
        {
            if (npcA != null) npcA.SetActive(false);
            if (npcB != null) npcB.SetActive(true);
        }
        else if (decision == "ShowBadNPC")
        {
            if (npcB != null) npcB.SetActive(false);
            if (npcA != null) npcA.SetActive(true);
        }
        else
        {
            if (npcA != null) npcA.SetActive(true);
            if (npcB != null) npcB.SetActive(true);
        }
    }
}

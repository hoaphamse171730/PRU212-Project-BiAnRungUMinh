using UnityEngine;

public class NPCSwitcher : MonoBehaviour
{
    [Header("Assign your NPC GameObjects")]
    public GameObject goodNPC;
    public GameObject badNPC;

    private void Start()
    {
        string decision = DecisionManager.SelectedEventID;

        if (decision == "ShowGoodNPC")
        {
            if (goodNPC != null) goodNPC.SetActive(true);
            if (badNPC != null) badNPC.SetActive(false);
        }
        else if (decision == "ShowBadNPC")
        {
            if (badNPC != null) badNPC.SetActive(true);
            if (goodNPC != null) goodNPC.SetActive(false);
        }
        else
        {
            if (goodNPC != null) goodNPC.SetActive(true);
            if (badNPC != null) badNPC.SetActive(true);
        }
    }
}

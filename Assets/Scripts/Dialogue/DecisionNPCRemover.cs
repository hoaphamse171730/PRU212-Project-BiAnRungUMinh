using UnityEngine;
using UnityEngine.Events;

public class DecisionNPCRemover : MonoBehaviour
{
    [Header("NPC to Remove")]
    [SerializeField] private GameObject npcToRemove;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;   // Ensure this AudioSource is on a persistent object
    [SerializeField] private AudioClip removalSound;

    [Header("Decision Manager Settings")]
    [SerializeField] private string decisionEventID = "removeNPC"; // Set this to match the decision mapping eventID

    private void Start()
    {
        // Wait until DecisionManager exists, then register this listener.
        // (You may want to delay registration if your scene loads asynchronously.)
        if (DecisionManager.Instance != null)
        {
            RegisterEvent();
        }
        else
        {
            // Optionally, you can start a coroutine that waits for the instance.
            StartCoroutine(WaitForDecisionManager());
        }
    }

    private System.Collections.IEnumerator WaitForDecisionManager()
    {
        while (DecisionManager.Instance == null)
        {
            yield return null;
        }
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        bool mappingFound = false;
        // Loop through existing mappings to find one matching the decisionEventID.
        foreach (var mapping in DecisionManager.Instance.decisionMappings)
        {
            if (mapping.eventID == decisionEventID)
            {
                mapping.decisionEvent.AddListener(RemoveNPCAndPlaySound);
                mappingFound = true;
                break;
            }
        }
        if (!mappingFound)
        {
            Debug.LogWarning($"No mapping found for decisionEventID: {decisionEventID}. " +
                             "Ensure a decision mapping with this ID is created in the DecisionManager.");
        }
    }

    public void RemoveNPCAndPlaySound()
    {
        // Play the sound effect first.
        if (audioSource != null && removalSound != null)
        {
            audioSource.PlayOneShot(removalSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or removalSound not assigned in DecisionNPCRemover.");
        }

        // Remove (destroy) the NPC.
        if (npcToRemove != null)
        {
            Destroy(npcToRemove);
        }
        else
        {
            Debug.LogWarning("NPC to Remove is not assigned in DecisionNPCRemover.");
        }
    }
}

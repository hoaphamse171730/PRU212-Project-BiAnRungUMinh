using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager Instance { get; private set; }
    // Static variable to store the chosen event ID between scenes.
    public static string SelectedEventID = "";

    [Header("Debug Info")]
    [SerializeField] private string debugSelectedEventID; // This field will appear in the Inspector

    [System.Serializable]
    public class DecisionMapping
    {
        public string eventID;
        public UnityEvent decisionEvent;
    }

    [SerializeField] private List<DecisionMapping> decisionMappings = new List<DecisionMapping>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        debugSelectedEventID = SelectedEventID;
    }

    public void TriggerDecision(string eventID)
    {
        // Save the decision for later use.
        SelectedEventID = eventID;

        foreach (var mapping in decisionMappings)
        {
            if (mapping.eventID == eventID)
            {
                Debug.Log("TriggerDecision called with eventID: " + eventID);
                mapping.decisionEvent.Invoke();
                return;
            }
        }
        Debug.LogWarning("No decision event found for eventID: " + eventID);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager Instance { get; private set; }

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

    public void TriggerDecision(string eventID)
    {
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

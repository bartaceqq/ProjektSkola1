using UnityEngine;

public class Collectible : MonoBehaviour
{
    private QuestManager questManager;

    public string questID;

    void Start()
    {
        questManager = Object.FindAnyObjectByType<QuestManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collectible triggered by: " + other.name);
        Debug.Log("Current Quest ID: " + questManager.currentQuest?.questID + ", Collectible Quest ID: " + questID);
        if (!other.CompareTag("Player"))
            return;

        if (questManager.currentQuest.questID != questID)
            return;

        questManager.AddQuestProgress(QuestType.CollectItems);
        Destroy(gameObject);
    }
}
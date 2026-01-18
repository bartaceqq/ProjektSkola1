using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] NPCs;
    public TextMeshProUGUI questDescription;
    public Slider questProgressSlider;

    public Quest[] incompleteQuests;
    public Quest currentQuest;

    public float interactionDistance = 3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        questProgressSlider.gameObject.SetActive(false);

        LoadQuestsFromJSON();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryAcceptQuest();
        }

        UpdateQuestProgressUI();
        if (currentQuest == null)
            return;
        if (currentQuest.currentAmount >= currentQuest.requiredAmount)
        {
            currentQuest.isCompleted = true;
            currentQuest = null;
        }
    }

    void TryAcceptQuest()
    {
        if (currentQuest != null)
            return;

        foreach (GameObject npc in NPCs)
        {
            if (Vector3.Distance(player.transform.position, npc.transform.position) > interactionDistance)
                continue;

            NPCScript questGiver = npc.GetComponent<NPCScript>();
            if (questGiver == null)
                continue;

            Quest quest = FindQuestByID(questGiver.questID);
            if (quest == null)
            {
                Debug.LogWarning("Quest ID not found: " + questGiver.questID);
                return;
            }

            currentQuest = quest;
            questDescription.text = quest.description;

            // Show slider for the new quest
            if (questProgressSlider != null)
            {
                questProgressSlider.gameObject.SetActive(true);
                questProgressSlider.maxValue = quest.requiredAmount;
                questProgressSlider.value = quest.currentAmount;
            }

            return;
        }
    }

    Quest FindQuestByID(string questID)
    {
        if (incompleteQuests == null) return null;

        foreach (Quest quest in incompleteQuests)
        {
            if (quest.questID == questID)
                return quest;
        }
        return null;
    }

    public void LoadQuestsFromJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("quests");

        if (jsonFile == null)
        {
            Debug.LogError("quests.json not found in Resources folder!");
            return;
        }

        QuestDataWrapper data = JsonUtility.FromJson<QuestDataWrapper>(jsonFile.text);

        if (data == null || data.quests == null || data.quests.Length == 0)
        {
            Debug.LogError("No quests found or failed to parse quests.json!");
            return;
        }

        incompleteQuests = new Quest[data.quests.Length];

        for (int i = 0; i < data.quests.Length; i++)
        {
            QuestData q = data.quests[i];
            incompleteQuests[i] = new Quest(
                q.questID,
                q.description,
                q.questType,
                q.requiredAmount
            );
        }

        Debug.Log("Quests loaded successfully. Total: " + incompleteQuests.Length);
    }

    public void AddQuestProgress(QuestType type, int amount = 1)
    {
        if (currentQuest == null) return;
        if (currentQuest.questType != type) return;

        currentQuest.AddProgress(amount);

        // Update slider immediately
        if (questProgressSlider != null)
        {
            questProgressSlider.value = currentQuest.currentAmount;
        }

        // Hide slider if quest completed
        if (currentQuest.isCompleted && questProgressSlider != null)
        {
            questProgressSlider.gameObject.SetActive(false);
        }
    }

    private void UpdateQuestProgressUI()
    {
        if (currentQuest == null || questProgressSlider == null)
            return;

        questProgressSlider.value = currentQuest.currentAmount;

        if (currentQuest.isCompleted)
        {
            questProgressSlider.gameObject.SetActive(false);
            questDescription.text = "";
            foreach (GameObject npc in NPCs)
            {
                NPCScript questGiver = npc.GetComponent<NPCScript>();
                if (questGiver != null && questGiver.questID == currentQuest.questID)
                {
                    questGiver.questID = "";
                    
                }
            }
        }
    }
}


[System.Serializable]
public class QuestData
{
    public string questID;
    public string description;
    public QuestType questType;
    public int requiredAmount;
}

[System.Serializable]
public class QuestDataWrapper
{
    public QuestData[] quests;
}

public enum QuestType
{
    CollectItems,
    KillCreatures
}

public class Quest
{
    public string questID;
    public string description;
    public QuestType questType;

    public int requiredAmount;
    public int currentAmount;
    public bool isCompleted;

    public Quest(string id, string desc, QuestType type, int required)
    {
        questID = id;
        description = desc;
        questType = type;
        requiredAmount = required;

        currentAmount = 0;
        isCompleted = false;
    }

    public void AddProgress(int amount = 1)
    {
        if (isCompleted) return;

        currentAmount += amount;
        if (currentAmount >= requiredAmount)
        {
            isCompleted = true;
            
        }
    }
}

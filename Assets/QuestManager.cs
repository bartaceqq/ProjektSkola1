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

        if (currentQuest != null && currentQuest.isCompleted)
        {
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
            {
                Debug.Log(questGiver.questID); 
            }
            
            Quest quest = FindQuestByID(questGiver.questID);
            for (int i = 0; i < 100; i++)
            {
                Debug.Log(quest.questType); 
            }
            if (quest == null)
            {
                Debug.LogWarning("Quest ID not found: " + questGiver.questID);
                return;
            }

           
            // Check if the quest is a boss fight and if previous quests are completed
            if (quest.questType == QuestType.BossFight)
            {
                /*
                if (!AreFirstTwoQuestsCompleted())
                {
                    Debug.Log("Complete the previous quests first!");
                    questDescription.text = "Complete the previous quests first!";
                    return;
                }
                */
            }
            else
            {
                currentQuest = quest;
                questDescription.text = quest.description;

                if (questProgressSlider != null)
                {
                    questProgressSlider.gameObject.SetActive(true);
                    questProgressSlider.maxValue = quest.requiredAmount;
                    questProgressSlider.value = quest.currentAmount;
                }

                return;
            }

          
        }
    }


    public Quest FindQuestByID(string questID)
    {
        if (incompleteQuests == null) return null;

        foreach (Quest quest in incompleteQuests)
        {
            if (quest.questID == questID)
            {
                Debug.Log("found quest: " + quest.description + " id: " + quest.questID + " type: " + quest.questType);
                return quest;
            }
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
        QuestType questType = (QuestType)System.Enum.Parse(typeof(QuestType), q.questType);
        incompleteQuests[i] = new Quest(
            q.questID,
            q.description,
            questType,
            q.requiredAmount
        );
    }

    Debug.Log("Quests loaded successfully. Total: " + incompleteQuests.Length);
}


    public void AddQuestProgress(QuestType type, int amount = 1)
    {
        if (currentQuest == null || currentQuest.questType != type)
            return;

        Debug.Log("Adding quest progress for " + type + ": " + amount);
        currentQuest.AddProgress(amount);

        if (questProgressSlider != null)
        {
            questProgressSlider.value = currentQuest.currentAmount;
        }

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
        }
    }

    public bool AreFirstTwoQuestsCompleted()
    {
        foreach (Quest quest in incompleteQuests)
        {
            if (quest.questID == "collect_01" && !quest.isCompleted)
                return false;
            if (quest.questID == "kill_01" && !quest.isCompleted)
                return false;
        }
        return true;
    }
}

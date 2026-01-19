    using System;

    [Serializable]
   
    public class QuestData
    {
        public string questID;
        public string description;
        public string questType; // Změna z QuestType na string
        public int requiredAmount;
    }


    [Serializable]
    public class QuestDataWrapper
    {
        public QuestData[] quests;
    }

    public enum QuestType
    {
        CollectItems,
        KillCreatures,
        BossFight
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
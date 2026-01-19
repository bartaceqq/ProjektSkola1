using UnityEngine;

public class BossSpawnerNPC : MonoBehaviour
{
    public GameObject bossPrefab;
    public QuestManager questManager;
    public float interactionDistance = 3f;
    public Transform spawnPoint;
    public GameObject player;
    public string questID = "boss_01";

    private bool _bossSpawned = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        questManager = FindAnyObjectByType<QuestManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerNear())
        {
            TrySpawnBoss();
        }
    }

    bool IsPlayerNear()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= interactionDistance;
    }

    public void TrySpawnBoss()
    {
        Quest quest = questManager.FindQuestByID(questID);
        if (quest == null)
        {
            Debug.LogError("Quest not found!");
            return;
        }

        if (!_bossSpawned && !questManager.AreFirstTwoQuestsCompleted())
        {
            Debug.Log("Complete the first two quests first!");
            questManager.questDescription.text = "Complete the first two quests first!";
            return;
        }

        if (!_bossSpawned)
        {
            SpawnBoss();
        }
        else
        {
            Debug.Log("Boss is already spawned!");
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && spawnPoint != null)
        {
            Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
            _bossSpawned = true;
            Debug.Log("Boss spawned!");
        }
        else
        {
            Debug.LogError("Boss prefab or spawn point not set!");
        }
    }
}
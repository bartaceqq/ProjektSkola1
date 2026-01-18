using UnityEngine;

public class RayCast : MonoBehaviour
{
    public float range = 100f;
    public LayerMask targetLayer;
    public Camera playerCamera;
    public KeyCode shootKey = KeyCode.Mouse0;

    private QuestManager questManager;

    void Start()
    {
        questManager = FindAnyObjectByType<QuestManager>();
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }

    void Shoot()
    {

        
        

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, range, targetLayer))
        {

            GameObject target = hit.collider.gameObject;
            Debug.Log("Hit target: " + target.name);

            // Only interact with creatures
            Creature creature = target.GetComponent<Creature>();
            if (creature == null)
                return;

            // Only apply damage if quest type matches
            if (questManager != null &&
                questManager.currentQuest != null &&
                questManager.currentQuest.questType == QuestType.KillCreatures)
            { 
                // Decrease creature HP
                creature.TakeDamage(1);

                // If creature is dead
                if (creature.IsDead())
                {
                    Destroy(target);
                    questManager.AddQuestProgress(QuestType.KillCreatures);
                }
            }
        }
    }
}

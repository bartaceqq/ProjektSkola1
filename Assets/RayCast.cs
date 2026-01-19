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
            Debug.Log("Hit: " + target.name);

            if (target.TryGetComponent(out Creature creature))
            {
                creature.TakeDamage(1);
                if (creature.IsDead()) 
                {
                    Destroy(target);
                    questManager.AddQuestProgress(QuestType.KillCreatures);
                }
            }
            else if (target.TryGetComponent(out Boss boss))
            {
                boss.TakeDamage(1);
                Debug.Log("Boss HP: " + boss.currentHP);
                if (boss.IsDead())
                {
                    Destroy(target);
                    questManager.AddQuestProgress(QuestType.BossFight);
                }
            }
        }
    }
}
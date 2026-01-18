using UnityEngine;

public class Creature : MonoBehaviour
{
    public int maxHP = 1;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0); // Clamp at 0
        Debug.Log(gameObject.name + " took " + amount + " damage. HP: " + currentHP);
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }
}

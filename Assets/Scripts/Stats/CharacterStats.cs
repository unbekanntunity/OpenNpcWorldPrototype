using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat currentHealth { get; private set; }

    public Stat Damage;
    public Stat Armor;

    void Awake()
    {
        currentHealth = new Stat();
        currentHealth.SetValue(maxHealth.GetValue());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10.0f);
        }
    }

    public void TakeDamage(float damage)
    {
        damage -= Armor.GetValue();

        if (damage > 0.0f)
            currentHealth.SetValue(currentHealth.GetValue() - damage);
        Debug.Log(transform.name + " takes " + damage + " damage");

        if (currentHealth.GetValue() <= 0.0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died");
    }

    public Stat GetArmor()
    {
        return Armor;
    }

    public Stat GetDamage()
    {
        return Damage;
    }
}

using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat currentHealth { get; private set; }

    public Stat Damage;
    public Stat Armor;

    public Weapon weapon;
    public Shield shield;
    public bool isBlocking;
    void Awake()
    {
        currentHealth = new Stat();
        currentHealth.SetValue(maxHealth.GetValue());
    }

    public void TakeDamage(GameObject attacker, float damage)
    {
        /*
        damage -= Armor.GetValue();
        */

        if (damage > 0.0f)
            currentHealth.SetValue(currentHealth.GetValue() - damage);
        Debug.Log(transform.name + " takes " + damage + " damage");
    }

    public Stat GetArmor()
    {
        return Armor;
    }

    public Stat GetDamage()
    {
        return Damage;
    }

    public Stat GetCurrentHealth()
    {
        return currentHealth;
    }

    public Stat GetMaxHealth()
    {
        return maxHealth;
    }
    public Weapon GetWeapon()
    {
        if (weapon != null)
            return weapon;
        else
            return null;
    }
    public Shield GetShield()
    {
        if (shield != null)
            return shield;
        else
            return null;
    }
}

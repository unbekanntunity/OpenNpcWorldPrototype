using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Items/ArmorItem")]
public class ArmorItem : Item
{
    public float armorModifier;
    public float damageModifier;

    override public void OnItemUsed(){
        
    }

    override public void OnItemEquipped()
    {

    }

    override public void OnItemUnEquipped()
    {

    }
}

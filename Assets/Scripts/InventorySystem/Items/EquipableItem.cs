using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipableItem", menuName = "Items/EquipableItem")]
public class EquipableItem : Item
{
    public enum EquipTypes
    {
        Helmet, Armor, Shield, Weapon, Shoe
    }

    public EquipTypes EquipType;

    override public void OnItemUsed(){
        
    }

    override public void OnItemEquipped()
    {

    }

    override public void OnItemUnEquipped()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhenAttacked : MonoBehaviour, IAttackable
{
    public EventHealthDecrease healthEvent;

    public void OnAttack(GameObject attacker, Attack attack)
    {
        healthEvent.Invoke((float)attack.Damage);
    }
}

[System.Serializable]
public class EventHealthDecrease : UnityEvent<float> { }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public int priority;
    public bool isTalking;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            GameObject obj = other.gameObject;
            InteractionScript script = obj.GetComponent<InteractionScript>();
            if (!script.isTalking)
            {
                if (priority > script.priority)
                {
                    if (Random.Range(0, 100) < 21)
                    {
                        isTalking = true;
                        script.isTalking = true;
                        Debug.Log("Send request");
                    }
                }
            }
        }
    }
}

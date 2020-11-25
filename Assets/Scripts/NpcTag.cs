using UnityEngine.Events;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class NpcTag : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        updateText();
     
    }
    private void Start()
    {
        var parent = GetComponentInParent<NpcData>();
        if (parent == null)
            enabled = false;

        var data = parent.OnNpcDataInspectorChanged;
        if (data == null)
            data = new UnityEvent();

        data.AddListener(updateText);
    }

    private void updateText()
    {
        text.text = GetComponentInParent<NpcData>().NpcName + "\nThe " + GetComponentInParent<NpcData>().Job.ToString().ToLower();
    }
}

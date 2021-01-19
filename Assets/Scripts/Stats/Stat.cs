using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float BaseValue;

    public float GetValue()
    {
        return BaseValue;
    }

    public void SetValue(float value)
    {
        BaseValue = value;
    }
}

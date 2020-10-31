using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="FloatReference")]
public class FloatReference : ScriptableObject
{
    public float value;

    public void SetValue(float v)
    {
        //Debug.Log("New value " + v);
        value = v;
    }
}

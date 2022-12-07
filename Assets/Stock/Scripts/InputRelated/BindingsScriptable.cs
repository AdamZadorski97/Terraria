using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

[CreateAssetMenu(fileName = "BindingProporties", menuName = "ScriptableObjects/BindingProporties", order = 1)]
public class BindingsScriptable : ScriptableObject
{
    public List<Binding> bindings;

    public Binding GetBinding(string name)
    {
        foreach(Binding binding in bindings)
        {
            if (binding.name == name) return binding;
        }
        Debug.LogError($"No binding with name {name}");
        return null;
    }
}
[Serializable]
public class Binding
{
    public string name;
    public InputControlType inputControlType;
    public Mouse mouse;
    public Key key;
}
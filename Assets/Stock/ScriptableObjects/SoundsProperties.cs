using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundsProperties", menuName = "ScriptableObjects/SoundsProperties", order = 1)]

public class SoundsProperties : ScriptableObject
{
    public List<Sound> Sounds;
}

[Serializable]
public class Sound
{
    public string name;
    public List<AudioClip> variants;
}

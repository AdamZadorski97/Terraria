using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableManager : MonoBehaviour
{

    public static ScriptableManager Instance { get; private set; }
    public BlockProperties blockProperties;
    public PlayerProperties playerProperties;
    public SoundsProperties soundsProperties;
    public WorldProperties worldProperties;
    public BindingProperties bindingProperties;
    public UserInterfaceProperties userInterfaceProperties;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sounds : MonoBehaviour
{
    public static P_Sounds Instance { get; private set; }
    private float currentFootstepTime;
    

    
    private AudioSource audioSource;
    private PlayerProperties playerProperties;
    private SoundsProperties soundProperties;
    private Sound footStepVariants;
    private int currentFootStepIndex;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        playerProperties = ScriptableManager.Instance.playerProperties;
        soundProperties = ScriptableManager.Instance.soundsProperties;
        footStepVariants = soundProperties.Sounds[0];
        SetupFootStepVariants("GrassFootStep");
    }

    public void SetupFootStepVariants(string name)
    {
        foreach(Sound sounds in soundProperties.Sounds)
        {
            if(name == sounds.name)
            {
                footStepVariants = sounds;
            }
        }
    }


    public void FootStep(float MoveValue)
    {
        currentFootstepTime += Time.deltaTime;

        float refreshTime = playerProperties.footStepRate * (Mathf.Abs(MoveValue) *0.1f);
        if (refreshTime < 0.25f)
            refreshTime = playerProperties.footStepRate;

        if (currentFootstepTime > refreshTime)
        {
            currentFootstepTime = 0;
            audioSource.PlayOneShot(footStepVariants.variants[currentFootStepIndex]);
            currentFootStepIndex++;
            if (currentFootStepIndex > footStepVariants.variants.Count-1)
            {
                currentFootStepIndex = 0;
            }
        }
    }



    public void PlaySound(string name,  float laudness = 1)
    {
        foreach (Sound sounds in soundProperties.Sounds)
        {
            if (name == sounds.name)
            {
                audioSource.PlayOneShot(sounds.variants[0], laudness);
            }
        }
    }



}

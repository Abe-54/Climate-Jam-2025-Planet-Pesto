using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SteamControllerPP : MonoBehaviour
{
    [Header("Steam Values")]
    public float maxSteam;
    public float currentSteam;
    
    
    [Header("UI Requirements")]
    public Slider steam;
    [SerializeField] float reductionSpeed = 1;

    private bool steamEmptyFlag = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!steam)
        {
            steam = GameObject.FindWithTag("SteamBar").gameObject.GetComponent<Slider>();
        }
        currentSteam = maxSteam;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!HasSteam() && !steamEmptyFlag)
        {
            EventBusPP<SteamEmptyEvent>.Raise(new SteamEmptyEvent());
            steamEmptyFlag = true;
        }

        UpdateSteamUI();
    }

    void UpdateSteamUI()
    {
        steam.value = currentSteam / maxSteam;
    }

    public void RemoveSteam(int amount)
    {
        currentSteam -= amount;
  

    }

    public void AddSteam(int amount)
    {
        currentSteam += amount;
        
    }
    
    public bool HasSteam() => currentSteam > 0;
}

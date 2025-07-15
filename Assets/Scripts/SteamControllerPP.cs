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
    [SerializeField] float reductionSpeed = 2f;
    private float newAmount;

    private bool steamEmptyFlag = false;
    private bool steamChanging = false;
    
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

        
        if (steamChanging)
        {
            currentSteam = Mathf.Floor(Mathf.Lerp(currentSteam, newAmount, Time.deltaTime)*1f)/1f;
            steam.value = currentSteam/maxSteam;
            if (currentSteam == newAmount)
            {
                steamChanging = false;
            }
       
        }
     
        

       
    }

    void UpdateSteamUI()
    {
        steam.value = currentSteam / maxSteam;
    }

    public void RemoveSteam(int amount)
    {
        newAmount = currentSteam - amount;
  
        steamChanging = true;

       
    }

    public void AddSteam(int amount)
    {
        currentSteam += amount;
        
    }
    
    public bool HasSteam() => currentSteam > 0;
}

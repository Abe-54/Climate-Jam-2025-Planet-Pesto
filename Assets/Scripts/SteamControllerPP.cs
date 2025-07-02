using UnityEngine;

public class SteamControllerPP : MonoBehaviour
{
    public float maxSteam;
    public float currentSteam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSteam = maxSteam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveSteam(int amount)
    {
        currentSteam -= amount;
    }
}

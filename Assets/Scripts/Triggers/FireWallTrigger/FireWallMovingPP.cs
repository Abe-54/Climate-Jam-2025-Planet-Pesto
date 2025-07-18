using System;
using UnityEngine;

public class FirewallMovingPP : MonoBehaviour
{
    [SerializeField] private float intensityLevelSpeed = 2f; // Speed of the firewall, can be adjusted as needed
    [SerializeField] private float intensityLevelSpeed2 = 4f; // Intensity level of the firewall, can be adjusted as needed

    // Update is called once per frame
    void Update()
    {
        if (GameManagerPP.instance.GetIntensityLevel() == 1)
        {
            MoveFirewall(intensityLevelSpeed); // Move the firewall at normal speed
        }
        else if (GameManagerPP.instance.GetIntensityLevel() == 2)
        {
            MoveFirewall(intensityLevelSpeed2); // Move the firewall at increased speed
        }
    }

    private void MoveFirewall(float speed)
    {
        gameObject.transform.Translate(Vector2.left * speed * Time.deltaTime); // Move the firewall left
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Handle player being hit by the firewall
            GameManagerPP.instance.PlayerHitByFirewall();
            // You can add more game over or damage logic here
        }
    }
}

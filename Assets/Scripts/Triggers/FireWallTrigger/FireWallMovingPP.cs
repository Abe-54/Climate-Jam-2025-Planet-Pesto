using UnityEngine;

public class FirewallMovingPP : MonoBehaviour
{
    [SerializeField] private float speed = 1f; // Speed of the firewall, can be adjusted as needed

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector2.left * speed * Time.deltaTime); // Move the firewall left
    }
}

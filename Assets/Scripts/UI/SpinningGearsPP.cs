using UnityEngine;
using UnityEngine.UI;

public class SpinningGearsPP : MonoBehaviour
{
    [SerializeField] private Image gear;
    [SerializeField] private float rotationSpeed;


    private void Start()
    {
        if (!gear)
        {
            gear = GetComponent<Image>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        gear.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }



}

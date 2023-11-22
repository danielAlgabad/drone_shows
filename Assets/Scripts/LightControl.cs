using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    [SerializeField] private Material lightColor;
    [SerializeField] private Material noLight;
    public float seconds = 0f;
    public bool activeBlink = false;
    private Material color;
    private bool activeLight = false;

    void Start()
    {
        if (activeBlink is true)
            InvokeRepeating("ChangeLight", 0f, seconds);
        else
            color = lightColor;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Drone")
        {
            if ((other.GetComponent<DroneMovement>().battery > 20f  && other.GetComponent<DroneMovement>().GetRecharging() is false))
                other.transform.Find("Light").GetComponent<MeshRenderer>().material = color;
        }
    }

    private void ChangeLight()
    {
        if (activeLight is true)
        {
            color = lightColor;
            activeLight = false;
        }
        else
        {
            color = noLight;
            activeLight = true;
        }
    }
}

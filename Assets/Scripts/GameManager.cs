using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public Button droneLogicButton;
    public Button changeAnimationStatusButton;
    public GameObject[] drones;
    public List<GameObject> dronesOnPosition;
    public Camera userCamera;
    private Vector3 cameraInitialPosition;
    private Quaternion cameraInitialRotation;
    private bool currentLogic = true;

    void Start()
    {
        drones = GameObject.FindGameObjectsWithTag("Drone");
        dronesOnPosition = new List<GameObject>();
        cameraInitialPosition = userCamera.transform.position;
        cameraInitialRotation = userCamera.transform.rotation;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

        foreach(GameObject drone in drones)
        {
            Vector3 distance = drone.transform.position - drone.GetComponent<DroneMovement>().GetCurrentObjective();
            if (distance.magnitude < 0.2)
            {
                if (dronesOnPosition.Contains(drone) is false)
                {
                    dronesOnPosition.Add(drone);
                }
            }
        }

        if (dronesOnPosition.Count == drones.Length)
        {
            dronesOnPosition.Clear();
            NextObjective();
        }
    }

    private void NextObjective()
    {
        foreach (GameObject drone in drones)
        {
            drone.GetComponent<DroneMovement>().GetNextObjective();
        }
    }

    public void ChangeDroneLogic()
    {
        if (currentLogic is true)
        {
            currentLogic = false;
            droneLogicButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sphere Cast";
            ChangeLogicForDrones();
        }
        else if (currentLogic is false)
        {
            currentLogic = true;
            droneLogicButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ray Cast";
            ChangeLogicForDrones();
        }
    }

    private void ChangeLogicForDrones()
    {
        foreach(GameObject drone in drones)
        {
            drone.GetComponent<DroneMovement>().SetMovementLogic(currentLogic);
        }
    }

    public void ChangeAnimationStatus()
    {
        foreach (GameObject drone in drones)
        {
            if(drone.GetComponent<DroneMovement>().GetActive() is false)
            {
                drone.GetComponent<DroneMovement>().StartAnimation();
                changeAnimationStatusButton.GetComponentInChildren<TextMeshProUGUI>().text = "End Animation";
            }
            else
            {
                drone.GetComponent<DroneMovement>().EndAnimation();
                changeAnimationStatusButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Animation";
            }
        }
    }

    public void ChangeToDragonAnimation()
    {
        SceneManager.LoadScene("DragonAnimation");
    }

    public void ChangeToDragonDebug()
    {
        SceneManager.LoadScene("DragonAnimationDEBUG");
    }

    public void ChangeToDanceAnimation()
    {
        SceneManager.LoadScene("DanceAnimation");
    }

    public void ChangeToDanceDebug()
    {
        SceneManager.LoadScene("DanceAnimationDEBUG");
    }

    public void ChangeToShipAnimation()
    {
        SceneManager.LoadScene("ShipAnimation");
    }

    public void ChangeToShipDebug()
    {
        SceneManager.LoadScene("ShipAnimationDEBUG");
    }

    public void ChangeToGearAnimation()
    {
        SceneManager.LoadScene("GearAnimation");
    }

    public void ChangeToGearDebug()
    {
        SceneManager.LoadScene("GearAnimationDEBUG");
    }

    public void ReturnToStart()
    {
        userCamera.transform.SetPositionAndRotation(cameraInitialPosition, cameraInitialRotation);
    }

}

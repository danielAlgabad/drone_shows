using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private Camera camera1;
    [SerializeField] private Camera camera2;
    [SerializeField] private Camera camera3;

    public void ChangeToCamera1()
    {
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
        camera1.gameObject.SetActive(true);
    }

    public void ChangeToCamera2()
    {
        camera1.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
    }

    public void ChangeToCamera3()
    {
        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(true);
    }
}

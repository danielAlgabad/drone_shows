using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastCollision : MonoBehaviour
{
    [SerializeField] private Material collisionMaterial;
    [SerializeField] private Material noCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Drone" && other != GetComponentInParent<Collider>())
            transform.GetComponent<MeshRenderer>().material = collisionMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Drone" && other != GetComponentInParent<Collider>())
            transform.GetComponent<MeshRenderer>().material = noCollision;
    }
}

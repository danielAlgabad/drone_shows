using UnityEngine;
using System.Collections.Generic;


public class DroneMovement : MonoBehaviour
{
    [SerializeField] private Transform[] objectives;
    [SerializeField] private Transform rechargeZone;
    [SerializeField] private Material blueLight;
    [SerializeField] private Material noLight;
    public float avoidanceRadius = 2f;
    public float speed = 1f;
    public float battery = 100f;
    public float batteryLoseRate = 1f;
    public float batteryGainRate = 1f;
    public float rayLength = 3f;
    public bool debug = false;
    private bool isActive = false;
    private bool movementLogic = true;
    private int index = 0;
    private Vector3 currentObjective;
    private Rigidbody rb;
    private bool recharging = false;
    private LayerMask mask;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        mask = LayerMask.GetMask("Objective");
        rb = GetComponent<Rigidbody>();
        GetNextObjective();
        currentObjective = GetCurrentObjective();
        InvokeRepeating("ConsumeBattery", 0f, 1f);
        InvokeRepeating("CalculateMovement", 0f, 0.01f);
        battery = Random.Range(100, 200);
    }

    private void CalculateMovement()
    {
        Vector3 seekForce = CalculateSeekForce();
        Vector3 avoidanceForce = Vector3.zero;

        if (movementLogic is true)
        {
            avoidanceForce = CalculateAvoidanceForceWithRayCast();
        }
        else if (movementLogic is false)
        {
            avoidanceForce = CalculateAvoidanceForceWithSphere();
        }

        Vector3 totalForce = avoidanceForce * 4 + seekForce;

        rb.AddForce(totalForce);
    }

    private Vector3 CalculateSeekForce()
    {
        Vector3 desiredForce = (currentObjective - transform.position).normalized * speed;
        Vector3 seekForce = desiredForce - rb.velocity;

        return seekForce;
    }

    private Vector3 CalculateAvoidanceForceWithRayCast()
    {
        Vector3 separationForce = Vector3.zero;

        RaycastHit hitForward;
        RaycastHit hitBehind;
        RaycastHit hitRightForwardUpAxis;
        RaycastHit hitLeftForwardUpAxis;
        RaycastHit hitRightUpAxis;
        RaycastHit hitLeftUpAxis;
        RaycastHit hitUp;
        RaycastHit hitDown;
        RaycastHit hitRightForwardRightAxis;
        RaycastHit hitLeftForwardRightAxis;
        RaycastHit hitRightRightAxis;
        RaycastHit hitLeftRightAxis;
        RaycastHit hitRightForwardForwardAxis;
        RaycastHit hitLeftForwardForwardAxis;
        RaycastHit hitRightForwardAxis;
        RaycastHit hitLeftForwardAxis;
        List<RaycastHit> hits = new List<RaycastHit>();

        PaintRayCast();

        Physics.SphereCast(transform.position, 1f, transform.forward, out hitForward, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, -transform.forward, out hitBehind, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(60, Vector3.up) * transform.forward, out hitRightForwardUpAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(-60, Vector3.up) * transform.forward, out hitLeftForwardUpAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(120, Vector3.up) * transform.forward, out hitRightUpAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(-120, Vector3.up) * transform.forward, out hitLeftUpAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(45, Vector3.right) * transform.up, out hitRightForwardRightAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(-45, Vector3.right) * transform.up, out hitLeftForwardRightAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(135, Vector3.right) * transform.up, out hitRightRightAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(-135, Vector3.right) * transform.up, out hitLeftRightAxis, rayLength, ~mask);   
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(45, Vector3.forward) * transform.up, out hitRightForwardForwardAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(-45, Vector3.forward) * transform.up, out hitLeftForwardForwardAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(135, Vector3.forward) * transform.up, out hitRightForwardAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, Quaternion.AngleAxis(-135, Vector3.forward) * transform.up, out hitLeftForwardAxis, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, transform.up, out hitUp, rayLength, ~mask);
        Physics.SphereCast(transform.position, 1f, -transform.up, out hitDown, rayLength, ~mask);


        hits.Add(hitForward);
        hits.Add(hitBehind);
        hits.Add(hitRightForwardUpAxis);
        hits.Add(hitLeftForwardUpAxis);
        hits.Add(hitRightUpAxis);
        hits.Add(hitLeftUpAxis);
        hits.Add(hitUp);
        hits.Add(hitDown);
        hits.Add(hitRightForwardRightAxis);
        hits.Add(hitLeftForwardRightAxis);
        hits.Add(hitRightRightAxis);
        hits.Add(hitLeftRightAxis);
        hits.Add(hitRightForwardForwardAxis);
        hits.Add(hitLeftForwardForwardAxis);
        hits.Add(hitRightForwardAxis);
        hits.Add(hitLeftForwardAxis);

        foreach (RaycastHit rayCastHit in hits)
        {
            if (rayCastHit.collider is not null)
            {
                Vector3 separationDirection = transform.position - rayCastHit.collider.transform.position;
                float separationDistance = separationDirection.magnitude;

                if (separationDistance < avoidanceRadius)
                {
                    separationForce += separationDirection.normalized * (avoidanceRadius - separationDistance);
                }
            }
        }

        return separationForce.normalized * speed;
    }

    private void PaintRayCast()
    {
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.white);
        Debug.DrawRay(transform.position, -transform.forward * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(60, Vector3.up) * transform.forward * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-60, Vector3.up) * transform.forward * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-120, Vector3.up) * transform.forward * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(120, Vector3.up) * transform.forward * rayLength, Color.white);
        Debug.DrawRay(transform.position, transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, -transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.right) * transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-45, Vector3.right) * transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-135, Vector3.right) * transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(135, Vector3.right) * transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.forward) * transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-45, Vector3.forward) * transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-135, Vector3.forward) * transform.up * rayLength, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(135, Vector3.forward) * transform.up * rayLength, Color.white);
    }

    private Vector3 CalculateAvoidanceForceWithSphere()
    {
        Vector3 separationForce = Vector3.zero;

        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, avoidanceRadius * 1.25f, ~mask);
        foreach (Collider collider in nearbyColliders)
        {
            if (collider != GetComponent<Collider>())
            {
                Vector3 separationDirection = transform.position - collider.transform.position;
                float separationDistance = separationDirection.magnitude;

                if (separationDistance < avoidanceRadius)
                {
                    separationForce += separationDirection.normalized * (avoidanceRadius - separationDistance);
                }
            }
        }

        return separationForce.normalized * speed;
    }

    private void ConsumeBattery()
    {
        if (recharging is false)
        {
            battery -= batteryLoseRate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RechargeZone")
            recharging = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "RechargeZone")
        {
            if (battery < 100)
                battery += batteryGainRate;
            if (battery >= 100)
            {
                recharging = false;
                GetNextObjective();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Drone")
            Debug.Log("Hit!");
    }

    public void GetNextObjective()
    {
        if (battery < 20f || recharging is true)
        {
            currentObjective = rechargeZone.position;
            if (debug is false)
            {
                transform.Find("Light").GetComponent<MeshRenderer>().material = noLight;
            }
        }
        else if (isActive is false)
        {
            currentObjective = initialPosition;
        }
        else
        {
            currentObjective = objectives[index].position;

            index = (index + 1) % objectives.Length;
        }
    }

    private void ChangeVisuals()
    {
        if (movementLogic is true)
        {
            transform.Find("RayCasts").gameObject.SetActive(true);
            transform.Find("SphereCast").gameObject.SetActive(false);
        }
        else if (movementLogic is false)
        {
            transform.Find("RayCasts").gameObject.SetActive(false);
            transform.Find("SphereCast").gameObject.SetActive(true);
        }
    }

    public Vector3 GetFirstPosition()
    {
        return objectives[0].position;
    }

    public Vector3 GetCurrentObjective()
    {
        return currentObjective;
    }

    public void SetMovementLogic(bool logic)
    {
        movementLogic = logic;
        if (debug is true)
            ChangeVisuals();
    }

    public bool GetActive()
    {
        return isActive;
    }

    public bool GetRecharging()
    {
        return recharging;
    }

    public void StartAnimation()
    {
        isActive = true;
        GetNextObjective();
        if (debug is false)
            transform.Find("Light").GetComponent<MeshRenderer>().material = noLight;
    }

    public void EndAnimation()
    {
        isActive = false;
        index = 0;
        GetNextObjective();
    }
}

using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SolarObject : MonoBehaviour
{
    //public float forceMagnitude;
    public double initialVelocityMagnitude = 0;
    public Vector3 initialVelocity = new Vector3(1, 0, 0);
    public double velocityScale = 10000;
    public OrbitalMass orbitalMass;
    public float radiusScale = 0;
    Rigidbody rb;
    public double scaledMass = 0;
    public bool canMove = true;
    readonly double gravitationalConstant = 6.67430e-11;
    public double force = 0;
    public double scaledForce = 0;
    CalculateMassDistances calc;
    public double forceScale = 1e24;
    public Vector3 forceDirection;
    public bool useForce = true;
    public float timeElapsed;
    public float orbitSpeed = 1;
    public SolarObject massToOrbit;
    public GameObject UIPanel;
    public TMP_Text PlanetNameTxt;
    public TMP_Text PlanetInfoTxt;
    public bool simulationPaused = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        calc = GetComponent<CalculateMassDistances>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //radiusScale = (float)calc.distanceScale;
        transform.localScale = (transform.localScale/2) * (orbitalMass.radius / radiusScale);
        orbitSpeed = (float)((orbitalMass.velocity/100000));
        if (canMove)
        {
            rb.angularVelocity = new Vector3(1,0,0) *  orbitalMass.angularVelocity;
            scaledMass = orbitalMass.mass / 1e24;
            rb.mass = (float)scaledMass;
            initialVelocityMagnitude = orbitalMass.velocity / velocityScale;
            initialVelocity = initialVelocity * (float)initialVelocityMagnitude;
            Debug.Log(initialVelocity);
            if (!float.IsInfinity((float)initialVelocityMagnitude))
            //rb.linearVelocity = initialVelocity;
            rb.AddForce(initialVelocity, ForceMode.Acceleration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!simulationPaused)
        {
            if (useForce)
            {
                MoveWithForce();
            }
            else
            {
                MoveWithEquation();
            }
        }
    }
    public void CalcScaledForce()
    {
        if(massToOrbit == null)
        {
            massToOrbit = SolarManager.Instance.sunObj.GetComponent<SolarObject>();
        }
        force = -gravitationalConstant * orbitalMass.mass * massToOrbit.orbitalMass.mass / math.pow(calc.GetScaledDistance() * 1000, 3);
        Vector3 heading = transform.position - massToOrbit.transform.position;
        float distance = heading.magnitude;
        forceDirection = heading;
        scaledForce = (force / forceScale);
        Debug.Log((float)scaledForce);
    }
    public void MoveWithForce()
    {
        if (canMove)
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }
            rb.AddForce(new Vector3(1, 0, 0), ForceMode.Force);
            CalcScaledForce();
            if (!float.IsInfinity((float)scaledForce))
                rb.AddForce(forceDirection * (float)scaledForce, ForceMode.Acceleration);
        }
    }
    public void MoveWithEquation()
    {
        if(!rb.isKinematic)
        {
            rb.isKinematic = true;
        }
        timeElapsed += Time.fixedDeltaTime;
        initialVelocityMagnitude = orbitalMass.velocity / velocityScale;
        Vector3 heading = SolarManager.Instance.sunLocation - transform.position;
        float distance = heading.magnitude;
        float x = SolarManager.Instance.sunLocation.x + (distance * Mathf.Cos(timeElapsed*orbitSpeed));
        float y = SolarManager.Instance.sunLocation.z + (distance * Mathf.Sin(timeElapsed * orbitSpeed));
        transform.position = new Vector3(x, transform.position.y, y);
    }
    public void OnRelease()
    {
        simulationPaused = false;
        Debug.Log("Check");
        if(useForce)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = true;
        }
        UIPanel.SetActive(false);
    }
    public void OnPickUp()
    {
        simulationPaused = true;
        UIPanel.SetActive(true);
        PlanetNameTxt.text = orbitalMass.name;
        PlanetInfoTxt.text = $"Planet Radius: {orbitalMass.radius}km\nPlanet Mass: {orbitalMass.mass}kg\nOrbital Velocity: {orbitalMass.velocity}m/s";
    }
}

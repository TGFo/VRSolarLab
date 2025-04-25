using UnityEngine;

public class CalculateMassDistances : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float currentDistance;
    public double scaledDistance;
    public double distanceScale = 10000000;
    public Transform orbitTransform;
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if (orbitTransform == null)
        {
            orbitTransform = SolarManager.Instance.sunObj.transform;
        }
        currentDistance = Vector3.Distance(transform.position, orbitTransform.position);
        
    }
    public double GetScaledDistance()
    {
        scaledDistance = currentDistance * distanceScale;
        return scaledDistance;
    }
    public double GetSquaredScaledDistance()
    {
        scaledDistance = Mathf.Pow(currentDistance, 2) * distanceScale;
        return scaledDistance;
    }
}

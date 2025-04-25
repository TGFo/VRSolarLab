using UnityEngine;

public class SolarManager : MonoBehaviour
{
    public static SolarManager Instance;
    public GameObject sunObj;
    public Vector3 sunLocation;
    public OrbitalMass sun;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        sunLocation = sunObj.transform.position;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

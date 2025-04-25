using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalMass", menuName = "Solar Bodies/OrbitalMass")]
public class OrbitalMass : ScriptableObject
{
    public double mass;
    public float radius;
    public float velocity;
    public float angularVelocity;
}

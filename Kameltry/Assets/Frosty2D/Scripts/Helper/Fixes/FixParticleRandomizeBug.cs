using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Helper/Fixes/FIX for Particle Randomization Bug")]
public class FixParticleRandomizeBug : MonoBehaviour
{
    public ParticleSystem ps;

    void Start()
    {
        ps.Stop();
        ps.randomSeed = (uint)Random.Range(0, uint.MaxValue);
        ps.Play();
    }
}

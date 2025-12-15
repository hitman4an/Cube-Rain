using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 100;
    [SerializeField] private float _explosionForce = 1000;

    public void Explode(Bomb bomb)
    {
        float maxExplosionForce = _explosionForce / bomb.transform.localScale.x;
        float minExplosionForce = _explosionForce;

        foreach (Rigidbody hit in GetExplodadbleObjects(bomb))
        {
            float distance = Vector3.Distance(bomb.transform.position, hit.position);
            float normalizedDistance = Mathf.InverseLerp(0, _explosionRadius, distance);
            float explosionForce = Mathf.Lerp(maxExplosionForce, minExplosionForce, normalizedDistance);
            hit.AddExplosionForce(explosionForce, bomb.transform.position, _explosionRadius);
        }
    }

    private List<Rigidbody> GetExplodadbleObjects(Bomb bomb)
    {
        Collider[] hits = Physics.OverlapSphere(bomb.transform.position, _explosionRadius);

        List<Rigidbody> cubes = new();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
                cubes.Add(hit.attachedRigidbody);
        }

        return cubes;
    }
}
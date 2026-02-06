using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    public float lifetime = 4f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}

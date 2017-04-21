using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Rigidbody rigidbody;
    public float damage = 1;
    public float force = 20;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rigidbody.AddRelativeForce(Vector3.forward * force, ForceMode.VelocityChange);
    }
    void OnCollisionEnter(Collision colision)
    {
        Destroy(gameObject);
        if (colision.gameObject.GetComponent<LivingEntity>() != null)
            colision.gameObject.GetComponent<LivingEntity>().TakeDamage(damage);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(1, transform.position, 1, 0, ForceMode.Impulse);
        }
    }
}

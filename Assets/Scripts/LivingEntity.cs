using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class LivingEntity : MonoBehaviour, IDamagable
{
    public event System.Action OnHit;
    public event System.Action OnDie;
    public ParticleSystem explosion;
    public Transform muzzle;
    public Projectile projectile;
    public float fireRate = 5;
    public float startHealth = 5;
    public float health { get; private set; }
    public float horizontalAcceleration = 4;
    public float verticalAcceleration = 3;
    public float deltaSpeed = 10;
    public float defaultSpeed = 0;
    Vector3 targetVelocity;
    Vector3 actualAcceleration;
    bool shooting;
    protected virtual void Start()
    {
        health = startHealth;
        shooting = false;
    }
    protected virtual void FixedUpdate()
    {

    }
    protected void Movement(float forward, float up)
    {
        targetVelocity = new Vector3(forward, up, 0);
        targetVelocity = targetVelocity.normalized;
        targetVelocity.x *= horizontalAcceleration;
        targetVelocity.y *= verticalAcceleration;
        targetVelocity = transform.TransformDirection(targetVelocity);
        transform.Translate(targetVelocity*Time.fixedDeltaTime);
    }
    protected void KeepBorders(float xMin, float xMax, float yMin, float yMax)
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax));
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (OnHit != null)
            OnHit();
        if (health <= 0)
        {
            if (OnDie != null)
                OnDie();
            OnDie = null;
            Destroy(gameObject);
            Destroy((Instantiate(explosion.gameObject, transform.position, Quaternion.identity) as GameObject).gameObject, 3);
        }
    }
    protected void Shoot()
    {
        if (!shooting)
            StartCoroutine(Shooting());
    }
    IEnumerator Shooting()
    {
        shooting = true;
        Instantiate(projectile, muzzle.position, muzzle.rotation);
        yield return new WaitForSeconds(1 / fireRate);
        shooting = false;
    }
}
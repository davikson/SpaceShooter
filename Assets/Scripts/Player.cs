using UnityEngine;
using System.Collections;


public class Player : LivingEntity
{
    protected override void Start()
    {
        base.Start();
        OnHit += AfterDamage;
    }
    protected override void FixedUpdate()
    {
        Movement(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        KeepBorders(-17, 17, -9, 9);
        if (Input.GetAxisRaw("Fire1") == 1)
            Shoot();
        base.FixedUpdate();
    }
    void AfterDamage()
    {
        StartCoroutine(ResetPosition());
    }
    IEnumerator ResetPosition()
    {
        transform.position = new Vector2(-12, 0);
        BoxCollider colider = GetComponent<BoxCollider>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Material material = GetComponent<Renderer>().material;
        Color color = material.color;
        colider.enabled = false;
        float time = 0;
        while(time < 2)
        {
            time += Time.fixedDeltaTime;
            material.color = Color.Lerp(color, Color.clear, Mathf.PingPong(time, .5f));
            yield return new WaitForFixedUpdate();
        }
        material.color = color;
        colider.enabled = true;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<LivingEntity>().TakeDamage(Mathf.Infinity);
            TakeDamage(1);
        }
    }
}

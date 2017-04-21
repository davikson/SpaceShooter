using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy : LivingEntity
{
    enum State {Fly, Attack, Avoid}
    State currentState;
    Player targetPlayer;
    Transform obstackle;
    public Canvas HealthBar;
    public Image Bar;
    public static event System.Action OnDying;


    protected override void Start()
    {
        base.Start();
        OnHit += UpdateHealthBar;
        OnDie += Dying;
        currentState = State.Fly;
        targetPlayer = FindObjectOfType<Player>();
    }
    protected override void FixedUpdate()
    {
        StateMachine();
        KeepBorders(-30, Mathf.Infinity, -9.5f, 9.5f);
        if (transform.position.x < -20)
            Destroy(gameObject);
        base.FixedUpdate();
    }
    void UpdateHealthBar()
    {
        HealthBar.transform.localScale = new Vector3(1, 1, 1);
        Bar.transform.localScale = new Vector3(health < 0 ? 0 : health / startHealth, 1, 1);
    }
    private void Dying()
    {
        if (OnDying != null)
            OnDying();
    }
    void StateMachine()
    {
        switch (currentState)
        {
            case State.Fly:
                if (targetPlayer != null && transform.position.x < 17 && transform.position.x > targetPlayer.transform.position.x)
                    currentState = State.Attack;
                else
                    foreach (Projectile projectile in FindObjectsOfType<Projectile>().ToList().OrderBy(item => Mathf.Abs(item.transform.position.y - transform.position.y)))
                    {
                        if (Mathf.Abs(projectile.transform.position.y - transform.position.y) < 1.0f)
                        {
                            currentState = State.Avoid;
                            break;
                        }
                    }
                break;
            case State.Attack:
                if (targetPlayer == null || transform.position.x < targetPlayer.transform.position.x)
                    currentState = State.Fly;
                else
                    foreach(Projectile projectile in FindObjectsOfType<Projectile>().ToList().OrderBy(item => Mathf.Abs(item.transform.position.y - transform.position.y)))
                    {
                        if (Mathf.Abs(projectile.transform.position.y - transform.position.y) < 0.5f && ((transform.position.x > projectile.transform.position.x && projectile.transform.rotation.y != transform.rotation.y) || (transform.position.x < projectile.transform.position.x && projectile.transform.rotation.y == transform.rotation.y)))//Mathf.Sign(transform.position.x-projectile.transform.position.x)==Mathf.Sign(projectile.GetComponent<Rigidbody>().velocity.x))
                        {
                            currentState = State.Avoid;
                            obstackle = projectile.transform;
                            break;
                        }
                    }
                break;
            case State.Avoid:
                if (obstackle == null)
                    currentState = State.Fly;
                else if (Mathf.Abs(obstackle.transform.position.y - transform.position.y) > 1.5f || obstackle.transform.position.x > transform.position.x)
                    currentState = State.Attack;
                break;
        }
        switch (currentState)
        {
            case State.Fly:
                Fly();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Avoid:
                Avoid();
                break;
        }
    }
    void Fly()
    {
        Movement(1, 0);
    }
    void Attack()
    {
        float yMovement;
        if (targetPlayer != null)
        {
            yMovement = Mathf.Clamp(targetPlayer.transform.position.y - transform.position.y, -1, 1);
            if (Mathf.Sign(yMovement) * Mathf.Sign(GetComponent<Rigidbody>().velocity.y) == 1)
                Movement(.5f, yMovement);
            else
                Movement(.5f, Mathf.Sign(yMovement));
            if (Mathf.Abs(yMovement) < .5f)
                Shoot();
        }

    }
    void Avoid()
    {
        float yMovement;
        if (obstackle != null)
        {
            yMovement = Mathf.Clamp(transform.position.y - obstackle.transform.position.y, -1, 1);
            yMovement = Mathf.Sign(yMovement) * (1 - Mathf.Abs(yMovement));
            Movement(.5f, yMovement);
        }
    }
    public static void resetOnDying()
    {
        OnDying = null;
    }
}

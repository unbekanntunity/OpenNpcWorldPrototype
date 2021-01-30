using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject Caster;
    private float Speed;
    private float Range;
    private Quaternion Direction;

    private float distanceTraveled;

    public event Action<GameObject, GameObject> ProjectileCollided;
    public void Fire(GameObject caster, Quaternion target, float speed, float range)
    {
        Caster = caster;
        Speed = speed;
        Range = range;

        Direction = target;
        transform.rotation = Direction;

        distanceTraveled = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        float distanceToTravel = Speed * Time.deltaTime;

        transform.Translate(Vector3.forward * distanceToTravel);

        distanceTraveled += distanceToTravel;

        if (distanceTraveled > Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("lol");
        if (ProjectileCollided != null)
        {
            ProjectileCollided(Caster, other.gameObject);
        }

        Destroy(gameObject);
    }
}

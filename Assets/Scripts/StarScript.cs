﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles a single background star's movement and destruction
/// </summary>
public class StarScript : MonoBehaviour
{
    private Rigidbody rig;
    private Transform trf;
    private Vector2 vel;
    private float depth = 0;

    private void Start()
    {
        depth = GetComponent<Transform>().position.z;
        Launch(1200 - depth * 6, depth / 20 + 2);
    }

    /// <summary>
    /// Lauch star to movement
    /// </summary>
    /// <param name="speed">star speed</param>
    /// <param name="lifeTime">how long should star remain in scene</param>
    public void Launch(float speed, float lifeTime)
    {
        Destroy(gameObject, lifeTime);
        trf = GetComponent<Transform>();
        vel = new Vector2(-speed, 0);
        rig = GetComponent<Rigidbody>();
        rig.velocity = vel;
    }
}
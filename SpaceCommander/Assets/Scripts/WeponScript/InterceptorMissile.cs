﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceCommander.Weapons
{
    class InterceptorMissile : SelfguidedMissile
    {
        public override void Start()
        {
            type = MissileType.Interceptor;
            Speed = 45f;
            DropImpulse = 250f;
            TurnSpeed = 100;
            explosionTime = 15f;
            AimCone = 120;
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * DropImpulse, ForceMode.Impulse);
            lt = 0;
        }
        protected override void Explode()
        {
            GameObject blast = Instantiate(FindObjectOfType<GlobalController>().ExplosiveBlast, this.transform.position, this.transform.rotation);
            blast.GetComponent<Explosion>().StatUp(BlastType.Missile);
            Destroy(gameObject);
        }
    }
}
﻿using SpaceCommander.Mechanics;
using UnityEngine;
namespace SpaceCommander.Mechanics.Weapons
{
    class HunterMissile : SelfguidedMissile
    {
        protected override void Start()
        {
            base.Start();
            body.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);
            lifeTime = 0;
        }
        public override void Explode()
        {
            GameObject blast = Instantiate(Global.Prefab.ExplosiveBlast, this.transform.position, this.transform.rotation);
            blast.GetComponent<Explosion>().StatUp(BlastType.Missile);
            Destroy(gameObject);
        }
    }
}

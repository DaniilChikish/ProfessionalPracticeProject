﻿using SpaceCommander.General;
using SpaceCommander.Mechanics;
using UnityEngine;
namespace SpaceCommander.Mechanics.Weapons
{
    class ThunderbolthHeavyRocket : SelfguidedMissile
    {
        private float fragRate = 32f;
        private float dispersion = 16f;
        private float fragSpeed = 200f;
        protected override void Start()
        {
            base.Start();
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * dropImpulse, ForceMode.Impulse);
            lifeTime = 0;
        }
        public override void Explode()
        {
            GameObject blast = Instantiate(GlobalController.Instance.Prefab.ExplosiveBlast, this.transform.position, this.transform.rotation);
            blast.GetComponent<Explosion>().StatUp(BlastType.Missile);
            float damage, armorPiersing, mass;
            bool canRicochet = true;
            GameObject explosionPrefab = null;
            damage = 30f;
            armorPiersing = 4;
            mass = 4f;
            Quaternion dispersionDelta;
            for (int i = 0; i < fragRate; i++)
            {
                dispersionDelta = WeaponBase.RandomDirectionNormal(dispersion);
                GameObject shell = Instantiate(GlobalController.Instance.Prefab.Buckshot, gameObject.transform.position, this.transform.rotation * dispersionDelta);
                shell.GetComponent<IShell>().StatUp(body.velocity + (fragSpeed * (dispersionDelta * this.transform.forward)), damage, armorPiersing, mass, canRicochet, explosionPrefab);
            }
            Destroy(gameObject);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceCommander.Weapons
{
    public class TorpedoLauncher : ShellWeapon
    {
        public TorpedoType AmmoType;
        protected override void StatUp()
        {
            base.StatUp();
            type = WeaponType.Torpedo;
            owner = this.transform.GetComponentInParent<SpaceShip>();
        }
        protected override void Shoot(Transform target)
        {
            GameObject missile;
            Transform targetTr = null;
            if (Target != null) targetTr = Target.transform;
            switch (AmmoType)
            {
                case TorpedoType.Nuke:
                    {
                        missile = Instantiate(Global.Torpedo, gameObject.transform.position, transform.rotation);
                        missile.AddComponent<NukeTorpedo>().SetTarget(targetTr);
                        break;
                    }
                case TorpedoType.Sprute:
                    {
                        missile = Instantiate(Global.Torpedo, gameObject.transform.position, transform.rotation);
                        missile.AddComponent<SpruteTorpedo>().SetTarget(targetTr);
                        break;
                    }
                case TorpedoType.ShieldsBreaker:
                    {
                        missile = Instantiate(Global.Torpedo, gameObject.transform.position, transform.rotation);
                        missile.AddComponent<ShieldBreakerTorpedo>().SetTarget(targetTr);
                        break;
                    }
                case TorpedoType.Thunderbolth:
                    {
                        missile = Instantiate(Global.Torpedo, gameObject.transform.position, transform.rotation);
                        missile.AddComponent<ThunderbolthHeavyRocket>().SetTarget(targetTr);
                        break;
                    }
                case TorpedoType.Unitary:
                default:
                    {
                        missile = Instantiate(Global.Torpedo, gameObject.transform.position, transform.rotation);
                        missile.AddComponent<UnitaryTorpedo>().SetTarget(targetTr);
                        break;
                    }
            }
            missile.GetComponent<Missile>().SetTeam(owner.Team);
            missile.GetComponent<Rigidbody>().AddForce(ownerBody.velocity, ForceMode.VelocityChange);
        }
    }
}

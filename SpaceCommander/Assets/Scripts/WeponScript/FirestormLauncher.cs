﻿using SpaceCommander.Mechanics;
using UnityEngine;
namespace SpaceCommander.Mechanics.Weapons
{
    public class FirestormLauncher : MagWeapon
    {
        public MissileType AmmoType;
        protected override void StatUp()
        {
            base.StatUp();
            type = WeaponType.Missile;
            audio.minDistance = 5;
            audio.maxDistance = 1000;
        }
        protected override void Shoot(Transform target)
        {
            GameObject missile;
            Quaternion dispersionDelta = RandomDirectionNormal(Dispersion);
            switch (AmmoType)
            {
                case MissileType.Interceptor:
                    {
                        missile = Instantiate(Global.Prefab.Missile, gameObject.transform.position, transform.rotation * dispersionDelta);
                        missile.AddComponent<InterceptorMissile>().SetTarget(target);
                        break;
                    }
                case MissileType.Hunter:
                    {
                         missile = Instantiate(Global.Prefab.Missile, gameObject.transform.position, transform.rotation * dispersionDelta);
                        missile.AddComponent<HunterMissile>().SetTarget(target);
                        break;
                    }
                case MissileType.Metheor:
                    {
                         missile = Instantiate(Global.Prefab.Missile, gameObject.transform.position, transform.rotation * dispersionDelta);
                        missile.AddComponent<MetheorMissile>().SetTarget(target);
                        break;
                    }
                case MissileType.Bombardier:
                default:
                    {
                        missile = Instantiate(Global.Prefab.Missile, gameObject.transform.position, transform.rotation * dispersionDelta);
                        missile.AddComponent<BombardierMissile>();
                        break;
                    }
            }
            missile.GetComponent<Rigidbody>().AddForce(ownerBody.velocity, ForceMode.VelocityChange);
        }
    }
}
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceCommander.Weapons
{
    public class Railgun : RoundWeapon {
        public override void StatUp()
        {
            type = WeaponType.Railgun;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            range = 1500;
            ammo = 15; //1.5 Min
            ammoCampacity = 15;
            firerate = 10;//200 DD, 40 DpS
            reloadingTime = 45;
            dispersion = 0.0f;
            shildBlinkTime = 0.05f;
            averageRoundSpeed = 300;
            PreAiming = true;
        }
        protected override void Shoot(Transform target)
        {
            float speed = 300f;
            float damage = 500f;
            float armorPiersing = 6f;
            float mass = 40f;

            Quaternion direction = transform.rotation;
            GameObject shell = Instantiate(Global.RailgunShell, gameObject.transform.position, direction);

            shell.GetComponent<IShell>().StatUp(speed * (1 + RoundspeedMultiplacator), damage * (1 + DamageMultiplacator), armorPiersing * (1 + APMultiplacator), mass * (1 + ShellmassMultiplacator), true, null);
        }
    }
}

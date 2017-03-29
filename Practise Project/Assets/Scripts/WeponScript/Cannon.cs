﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace PracticeProject
{
    public class Cannon : Weapon
    {
        protected override void Start()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            range = 100;
            ammo = 2000;
            coolingTime = 0.3f;
            cooldown = 0;
            dispersion = 0.5f;
            shildBlinkTime = 0.01f;
        }
        protected override void Shoot(Transform target)
        {
			Global = FindObjectsOfType<GlobalController>()[0];
            Quaternion direction = transform.rotation;
            double[] randomOffset = Randomizer.Uniform(0, 100, 2);
            if (randomOffset[0] > 50)
                direction.x = direction.x + (Convert.ToSingle(Global.RandomNormalPool[randomOffset[0]] - RandomNormalMin) * dispersion);
            else
                direction.x = direction.x + (Convert.ToSingle(Global.RandomNormalPool[randomOffset[0]] - RandomNormalMin) * -dispersion);
            if (randomOffset[1] > 50)
                direction.y = direction.y + (Convert.ToSingle(Global.RandomNormalPool[randomOffset[1]] - RandomNormalMin) * dispersion);
            else
                direction.y = direction.y + (Convert.ToSingle(Global.RandomNormalPool[randomOffset[1]] - RandomNormalMin) * -dispersion);
            Instantiate(Global.CannonUnitaryShell, gameObject.transform.position, direction);
        }
    }
}

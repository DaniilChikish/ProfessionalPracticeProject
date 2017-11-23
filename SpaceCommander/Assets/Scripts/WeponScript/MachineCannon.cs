﻿using SpaceCommander.Mechanics;
using UnityEngine;
/**
* Автоматическая пушка
* Физические параметры: (по образу ШВАК)
*      Скорострельность = 700 в/м
*      Начальная скорость ~ 800 м/с
*      Боезапас ~ 150
*      Масса снаряда ~ 0.2кг
* **/
namespace SpaceCommander.Mechanics.Weapons
{
    public class MachineCannon : MagWeapon
    {
        public ShellLineType AmmoType;
        public SmallShellType[] ShellLine;
        public int shellPosition;
        private float heat;
        protected override void StatUp()
        {
            base.StatUp();
            type = WeaponType.MachineCannon;
            audio.minDistance = 5;
            audio.maxDistance = 1000;
            switch (AmmoType)
            {
                case ShellLineType.Solid:
                    {
                        ShellLine = new SmallShellType[5];
                        ShellLine[0] = SmallShellType.SemiShell;
                        ShellLine[1] = SmallShellType.Solid;
                        ShellLine[2] = SmallShellType.Solid;
                        ShellLine[3] = SmallShellType.Solid;
                        ShellLine[4] = SmallShellType.APShell;
                        break;
                    }
                case ShellLineType.ArmorPenetration:
                    {
                        ShellLine = new SmallShellType[4];
                        ShellLine[0] = SmallShellType.Solid;
                        ShellLine[1] = SmallShellType.APShell;
                        ShellLine[2] = SmallShellType.Incendiary;
                        ShellLine[3] = SmallShellType.APShell;
                        break;
                    }
                case ShellLineType.ShildOwerheat:
                    {
                        ShellLine = new SmallShellType[6];
                        ShellLine[0] = SmallShellType.SemiShell;
                        ShellLine[1] = SmallShellType.SemiShell;
                        ShellLine[2] = SmallShellType.SemiShell;
                        ShellLine[3] = SmallShellType.Solid;
                        ShellLine[4] = SmallShellType.Incendiary;
                        ShellLine[5] = SmallShellType.Solid;
                        break;
                    }
                case ShellLineType.Incendiary:
                    {
                        ShellLine = new SmallShellType[4];
                        ShellLine[0] = SmallShellType.Solid;
                        ShellLine[1] = SmallShellType.Incendiary;
                        ShellLine[2] = SmallShellType.APShell;
                        ShellLine[3] = SmallShellType.Incendiary;
                        break;
                    }
                case ShellLineType.Universal:
                    {
                        ShellLine = new SmallShellType[6];
                        ShellLine[0] = SmallShellType.SemiShell;
                        ShellLine[1] = SmallShellType.Solid;
                        ShellLine[2] = SmallShellType.APShell;
                        ShellLine[3] = SmallShellType.SemiShell;
                        ShellLine[4] = SmallShellType.Incendiary;
                        ShellLine[5] = SmallShellType.APShell;
                        break;
                    }
            }
            shellPosition = 0;
        }
        protected override void UpdateLocal()
        {
            if (heat > 0) heat = heat * 0.98f;
        }
        protected override void Shoot(Transform target)
        {
            heat += 1;
            float localDisp = Dispersion + (Dispersion * heat / 1);
            Quaternion dispersionDelta = RandomDirectionNormal(localDisp);

            GameObject shell = Instantiate(Global.Prefab.UnitaryShell, gameObject.transform.position, this.transform.rotation * dispersionDelta);
            if (shellPosition >= ShellLine.Length)
                shellPosition = 0;

            float damage, armorPiersing, mass;
            bool canRicochet = false;
            GameObject explosionPrefab = null;
            switch (ShellLine[shellPosition])
            {
                case SmallShellType.SemiShell:
                    {
                        damage = 25f;
                        armorPiersing = 1.5f;
                        mass = 0.18f;
                        break;
                    }
                case SmallShellType.APShell:
                    {
                        damage = 15f;
                        armorPiersing = 4;
                        mass = 0.22f;
                        canRicochet = true;
                        break;
                    }
                case SmallShellType.Incendiary:
                    {
                        damage = 5f;
                        armorPiersing = 1;
                        mass = 0.2f;
                        explosionPrefab = Global.Prefab.ShellBlast;
                        break;
                    }
                case SmallShellType.BuckShot:
                    {
                        damage = 30f;
                        armorPiersing = 1;
                        mass = 0.3f;
                        canRicochet = true;
                        break;
                    }
                case SmallShellType.Solid:
                default:
                    {
                        damage = 20f;
                        armorPiersing = 2;
                        mass = 0.2f;
                        canRicochet = true;
                        break;
                    }
            }

            shell.GetComponent<General.IShell>().StatUp(owner.Velocity + (RoundSpeed * (dispersionDelta * this.transform.forward)), damage * (1 + DamageMultiplacator), armorPiersing * (1 + APMultiplacator), mass * (1 + ShellmassMultiplacator), canRicochet, explosionPrefab);
            shellPosition++;
            ownerBody.AddForceAtPosition(-this.transform.forward * mass * RoundSpeed, this.transform.position, ForceMode.Impulse);
        }
    }
}


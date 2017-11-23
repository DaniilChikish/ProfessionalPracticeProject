﻿using SpaceCommander.AI;
using SpaceCommander.Mechanics.Modules;
using UnityEngine;
namespace SpaceCommander.Mechanics.Units
{
    public class Support_Corvette : SpaceShip
    {

        private bool idleFulag;
        protected override void StatsUp()
        {
            base.StatsUp();
            type = UnitClass.Support_Corvette;
            EnemySortDelegate = SupportCorvetteSortEnemys;
            AlliesSortDelegate = SupportCorvetteSortEnemys;
            module = new SpellModule[4];
            module[0] = new EmergencySelfRapairing(this);
            module[1] = new EmergencyShieldRecharging(this);
            module[2] = new TurretPlant(this);
            module[3] = new BattleDroneLauncher(this);
        }
        protected override void Explosion()
        {
            GameObject blast = Instantiate(Global.Prefab.ShipDieBlast, gameObject.transform.position, gameObject.transform.rotation);
            blast.GetComponent<Explosion>().StatUp(BlastType.Corvette);
        }
        protected override void DecrementLocalCounters()
        {

        }
        protected override bool IdleManeuverFunction()
        {
            idleFulag = !idleFulag;
            if (idleFulag)
                return Driver.ExecetePointManeuver(PointManeuverType.PatroolLine, this.transform.position, this.transform.right * 150);
            else return Driver.ExecetePointManeuver(PointManeuverType.PatroolDiamond, this.transform.position, this.transform.forward * 50);
        }
    }
}

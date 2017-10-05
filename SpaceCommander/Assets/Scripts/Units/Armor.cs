﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceCommander
{
    public class Armor : MonoBehaviour
    {
        [SerializeField]
        public float maxHitpoints;
        [SerializeField]
        public float hitpoints;
        [SerializeField]
        public float shellResist;
        [SerializeField]
        public float energyResist;
        [SerializeField]
        public float blastResist;
        private float hitCount;
        private SpaceShip owner;
        private GlobalController Global;
        // Use this for initialization
        public float ShellResist { get { return this.shellResist * (1 + owner.ResistMultiplacator); } }
        public float EnergyResist { get { return this.energyResist * (1 + owner.ResistMultiplacator); } }
        public float BlastResist { get { return this.blastResist * (1 + owner.ResistMultiplacator); } }
        void Start()
        {
            owner = transform.GetComponentInParent<SpaceShip>();
            Global = FindObjectOfType<GlobalController>();
            //Global.SpecINI.Write(owner.GetType().ToString(), "maxHitpoints", maxHitpoints.ToString());
            //Global.SpecINI.Write(owner.GetType().ToString(), "shellResist", shellResist.ToString());
            //Global.SpecINI.Write(owner.GetType().ToString(), "energyResist", energyResist.ToString());
            //Global.SpecINI.Write(owner.GetType().ToString(), "blastResist", blastResist.ToString());

            maxHitpoints = Convert.ToSingle(Global.SpecINI.ReadINI(owner.GetType().ToString(), "maxHitpoints"));
            shellResist = Convert.ToSingle(Global.SpecINI.ReadINI(owner.GetType().ToString(), "shellResist"));
            energyResist = Convert.ToSingle(Global.SpecINI.ReadINI(owner.GetType().ToString(), "energyResist"));
            blastResist = Convert.ToSingle(Global.SpecINI.ReadINI(owner.GetType().ToString(), "blastResist"));

            hitpoints = maxHitpoints;
        }

        // Update is called once per frame
        void Update()
        {
            if (hitpoints < 0)
                owner.Die();
            else if (hitpoints < maxHitpoints * 0.1)
                hitpoints -= Time.deltaTime;
            else if (hitpoints < maxHitpoints * 0.3)
                owner.ArmorCriticalAlarm();
        }
        protected void OnCollisionEnter(Collision collision)
        {
            float multiplicator;
            switch (collision.gameObject.tag)
            {
                case "Shell":
                    {
                        hitCount += 1;
                        //Debug.Log(collision.gameObject.name + " hit " + owner.name);
                        float difference = collision.gameObject.GetComponent<IShell>().ArmorPiersing - ShellResist;
                        if (difference > 1.5)
                            multiplicator = 1.2f;
                        else if (difference > -3)
                            multiplicator = (Mathf.Sin((difference / 1.4f) + 0.5f) + 1f) * 0.6f;
                        else
                            multiplicator = 0.0f;
                        if (!owner.ShieldOwerheat) multiplicator = multiplicator * (1 - owner.ShieldForce / owner.ShieldCampacity);
                        this.hitpoints -= collision.gameObject.GetComponent<IShell>().Damage * multiplicator;
                        break;
                    }
                case "Energy":
                    {
                        hitCount += 3;
                        float difference = collision.gameObject.GetComponent<IEnergy>().ArmorPiersing - EnergyResist;
                        if (difference > 0.6)
                            multiplicator = 1f;
                        else if (difference > -2.8)
                            multiplicator = (Mathf.Sin((difference / 1.1f) + 1f) + 1f) * 0.5f;
                        else
                            multiplicator = 0.0f;
                        if (!owner.ShieldOwerheat) multiplicator = multiplicator * 0.3f;
                        this.hitpoints -= collision.gameObject.GetComponent<IEnergy>().Damage * multiplicator;
                        break;
                    }
                case "Unit":
                    {
                        Unit uncknown = collision.gameObject.GetComponent<Unit>();
                        Vector3 normalSum = Vector3.zero;
                        foreach (var x in collision.contacts)
                            normalSum += x.normal;
                        Vector3 kick = Vector3.Project(uncknown.Velocity - owner.Velocity, normalSum.normalized);
                        this.hitpoints -= kick.magnitude;
                        break;
                    }
                case "Terrain":
                    {
                        Vector3 normalSum = Vector3.zero;
                        foreach (var x in collision.contacts)
                            normalSum += x.normal;
                        Vector3 kick = Vector3.Project(owner.Velocity, normalSum.normalized);
                        this.hitpoints -= kick.magnitude;
                        //this.transform.position = Vector3.MoveTowards(this.transform.position, normalSum * 5, Time.deltaTime * owner.ShiftSpeed * 0.2f);
                        break;
                    }
            }
        }
        protected void OnCollisionStay(Collision collision)
        {
            //Debug.Log("Hit armor");
            //if (owner.GetShieldRef.force < 0)
            //{
            float multiplicator;
            switch (collision.gameObject.tag)
            {
                //case "Shell":
                //    {
                //        float difference = collision.gameObject.GetComponent<IShell>().ArmorPiersing - shellResist;
                //        if (difference > 1.5)
                //            multiplicator = 1.2f;
                //        else if (difference > -3)
                //            multiplicator = (Mathf.Sin((difference / 1.4f) + 0.5f) + 1f) * 0.6f;
                //        else
                //            multiplicator = 0.0f;
                //        this.hitpoints -= collision.gameObject.GetComponent<IShell>().Damage * multiplicator;
                //        break;
                //    }
                case "Energy":
                    {
                        hitCount += 3;
                        float difference = collision.gameObject.GetComponent<IEnergy>().ArmorPiersing - EnergyResist;
                        if (difference > 0.5)
                            multiplicator = 1f;
                        else if (difference > -3)
                            multiplicator = (Mathf.Sin((difference / 1.1f) + 1f) + 1f) * 0.5f;
                        else
                            multiplicator = 0.0f;
                        this.hitpoints -= collision.gameObject.GetComponent<IEnergy>().Damage * multiplicator * Time.deltaTime;
                        break;
                    }
                case "Explosion":
                    {
                        multiplicator = (1f - BlastResist) * Mathf.Pow(((-Vector3.Distance(this.gameObject.transform.position, collision.gameObject.transform.position) + collision.gameObject.GetComponent<Explosion>().MaxRadius) * 0.01f), (1f / 3f));
                        this.hitpoints -= collision.gameObject.GetComponent<Explosion>().Damage * multiplicator;
                        break;
                    }
            }
            //}
        }
        protected void OnTriggerStay(Collider trigger)
        {
            float multiplicator;
            switch (trigger.gameObject.tag)
            {
                case "Energy":
                    {
                        //Debug.Log(this.owner.name + ": Laser pierse by" + trigger.gameObject.GetComponentInParent<SpaceShip>().name);
                        hitCount += 3;
                        float difference = trigger.gameObject.GetComponent<IEnergy>().ArmorPiersing - EnergyResist;
                        if (difference > 0.5)
                            multiplicator = 1f;
                        else if (difference > -3)
                            multiplicator = (Mathf.Sin((difference / 1.1f) + 1f) + 1f) * 0.5f;
                        else
                            multiplicator = 0.0f;
                        this.hitpoints -= trigger.gameObject.GetComponent<IEnergy>().Damage * multiplicator * Time.deltaTime;
                        break;
                    }
                case "Explosion":
                    {
                        float sizeFactor = 0;
                        if (owner.Type == UnitClass.LR_Corvette)
                            sizeFactor = 10;
                        else if (owner.Type == UnitClass.Guard_Corvette || owner.Type == UnitClass.Support_Corvette)
                            sizeFactor = 5;
                        if (Vector3.Distance(trigger.transform.position, this.transform.position) < trigger.gameObject.GetComponent<Explosion>().MaxRadius + sizeFactor)
                        {
                            multiplicator = (1 - BlastResist) * (trigger.gameObject.GetComponent<Explosion>().MaxRadius + sizeFactor - Vector3.Distance(trigger.transform.position, this.transform.position)) / trigger.gameObject.GetComponent<Explosion>().MaxRadius;
                            if (multiplicator > 1) multiplicator = 1;
                            if (multiplicator < 0) multiplicator = 0;
                            this.hitpoints -= trigger.gameObject.GetComponent<Explosion>().Damage * multiplicator;
                        }
                        break;
                    }
            }
        }
    }
}

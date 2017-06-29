﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCommander
{
    public class ForceShield : MonoBehaviour
    {
        public float maxCampacity;
        public float force;
        public float recharging;
        public bool isOwerheat;
        public float cooldownChield;
        public float firstBlinker;
        public float secondBlinker;
        MeshRenderer firstFieldRend;
        MeshCollider firstFieldColl;
        MeshRenderer secondField;
        ParticleSystem shildCollaps;
        // Use this for initialization
        void Start()
        {
            firstFieldRend = this.transform.FindChild("FirstField").GetComponent<MeshRenderer>();
            firstFieldColl = this.transform.FindChild("FirstField").GetComponent<MeshCollider>();
            shildCollaps = this.transform.FindChild("FirstField").GetComponentInChildren<ParticleSystem>();
            secondField = this.transform.FindChild("SecondField").GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (force < 0 && !isOwerheat)
                Owerheat();
            else if (cooldownChield <= 0 && secondBlinker <= 0 && force < maxCampacity)
            {
                force += recharging * Time.deltaTime;
            }
            if (cooldownChield > 0)
                cooldownChield -= Time.deltaTime;
            if (!isOwerheat)
            {
                if (firstBlinker <= 0)
                    firstFieldColl.enabled = true;
                else firstBlinker -= Time.deltaTime;
            }
            else if (force > maxCampacity * 0.1)
                Reload();
            if (secondBlinker <= 0)
                secondField.enabled = false;
            else secondBlinker -= Time.deltaTime;
        }
        //protected void OnCollisionEnter(Collision collision)
        //{
        //    //Debug.Log("Hit shield");
        //    if (!shildOwerheat)
        //    {
        //        switch (collision.gameObject.tag)
        //        {
        //            case "Shell":
        //                {
        //                    this.force -= collision.gameObject.GetComponent<Round>().Damage * 0.3f;
        //                    secondField.enabled = true;
        //                    secondBlinker = 0.5f;
        //                    break;
        //                }
        //            case "Energy":
        //                {
        //                    this.force -= collision.gameObject.GetComponent<Round>().Damage * 1.5f;
        //                    secondField.enabled = true;
        //                    secondBlinker = 0.5f;
        //                    break;
        //                }
        //            case "Missile":
        //                {
        //                    secondField.enabled = true;
        //                    secondBlinker = 0.5f;
        //                    break;
        //                }
        //        }
        //    }
        //}
        protected void OnTriggerStay(Collider collision)
        {

            if (!isOwerheat)
            {
                switch (collision.gameObject.tag)
                {
                    case "Shell":
                        {
                            Rigidbody shell = collision.GetComponent<Rigidbody>();
                            this.force -= shell.mass * 1.5f;
                            secondField.enabled = true;
                            secondBlinker = 0.5f;
                            shell.AddForce((collision.transform.position-this.transform.position).normalized * Mathf.Sqrt(shell.mass * maxCampacity * 20), ForceMode.Impulse);//velocity = collision.GetComponent<Rigidbody>().velocity / 2;
                            break;
                        }
                    case "Energy":
                        {
                            this.force -= collision.gameObject.GetComponent<IEnergy>().GetEnergy();
                            secondField.enabled = true;
                            secondBlinker = 0.5f;
                            break;
                        }
                    case "Missile":
                        {
                            secondField.enabled = true;
                            Rigidbody shell = collision.GetComponent<Rigidbody>();
                            collision.GetComponent<SelfguidedMissile>().Arm();
                            secondBlinker = 1.5f;
                            shell.AddForce((collision.transform.position - this.transform.position).normalized * Mathf.Sqrt(shell.mass * maxCampacity * 10), ForceMode.Force);//velocity = collision.GetComponent<Rigidbody>().velocity / 2;
                            break;
                        }
                    case "Explosion":
                        {
                            this.force = this.force - collision.gameObject.GetComponent<Explosion>().Damage * 0.01f;
                            break;
                        }
                }
            }
        }
        public void Blink(float blink)
        {
            firstBlinker = blink;
            firstFieldColl.enabled = false;
        }
        public void Owerheat()
        {
            isOwerheat = true;
            firstFieldRend.enabled = false;
            firstFieldColl.enabled = false;
            shildCollaps.Play();
            force = 0;
            cooldownChield = 2;
        }
        public void Reload()
        {
            isOwerheat = false;
            firstFieldRend.enabled = true;
            firstFieldColl.enabled = true;
        }
    }
}

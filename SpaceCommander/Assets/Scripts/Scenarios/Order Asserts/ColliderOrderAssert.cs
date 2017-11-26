﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpaceCommander.Scenarios
{
    class ColliderOrderAssert : OrderAssert
    {
        public enum Functions
        {
            OnControllerColliderHit,
            OnParticleCollision,
            OnJointBreak,
            OnTriggerEnter,
            OnTriggerExit,
            OnTriggerStay,
            OnCollisionEnter,
            OnCollisionExit,
            OnCollisionStay,
            //OnTriggerEnter2D,
            //OnTriggerExit2D,
            //OnTriggerStay2D,
            //OnCollisionEnter2D,
            //OnCollisionExit2D,
            //OnCollisionStay2D,
        }
        public OrderAssert previous;
        [SerializeField]
        private Functions callOnMethod;

        [SerializeField]
        private float triggetRange;
        private const float checkRate = 1f;
        private ColliderAssertMarker[] markers;
        private float backCount;
        private void TryToCallOrder(Functions invokingMethod)
        {
            if (invokingMethod == callOnMethod && (previous == null || (previous != null && previous.State == OrderAccertState.Complete)))
            {
                if (methodToCall == Method.Pass)
                    State = OrderAccertState.Complete;
                else
                    State = OrderAccertState.Fail;
            }
        }
        private void Start()
        {
            if (callOnMethod == Functions.OnTriggerEnter || callOnMethod == Functions.OnTriggerStay)
            {
                ColliderAssertMarker[] bufferA = FindObjectsOfType<ColliderAssertMarker>();
                List<ColliderAssertMarker> bufferB = new List<ColliderAssertMarker>();
                foreach (var x in bufferA)
                {
                    if (x.dependence.Priority == this.Priority)
                        bufferB.Add(x);
                }
                markers = bufferB.ToArray();
                backCount = checkRate;
            }
        }
        private void Update()
        {
            if ((callOnMethod == Functions.OnTriggerEnter || callOnMethod == Functions.OnTriggerStay) && backCount <= 0)
            {
                foreach (var x in markers)
                {
                    if (Vector3.Distance(x.gameObject.transform.position, this.gameObject.transform.position) <= triggetRange)
                    {
                        TryToCallOrder(Functions.OnTriggerStay);
                        return;
                    }
                }
                backCount = checkRate;
            }
            else backCount -= Time.deltaTime;
        }
        public void OnControllerColliderHit()
        {
            TryToCallOrder(Functions.OnControllerColliderHit);
        }
        public void OnParticleCollision()
        {
            TryToCallOrder(Functions.OnParticleCollision);
        }
        public void OnJointBreak()
        {
            TryToCallOrder(Functions.OnJointBreak);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ColliderAssertMarker>() != null && other.GetComponent<ColliderAssertMarker>().dependence == this)
                TryToCallOrder(Functions.OnTriggerEnter);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ColliderAssertMarker>() != null && other.GetComponent<ColliderAssertMarker>().dependence == this)
                TryToCallOrder(Functions.OnTriggerExit);
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<ColliderAssertMarker>() != null && other.GetComponent<ColliderAssertMarker>().dependence == this)
                TryToCallOrder(Functions.OnTriggerStay);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<ColliderAssertMarker>() != null && collision.gameObject.GetComponent<ColliderAssertMarker>().dependence == this)
                TryToCallOrder(Functions.OnCollisionEnter);
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<ColliderAssertMarker>() != null && collision.gameObject.GetComponent<ColliderAssertMarker>().dependence == this)
                TryToCallOrder(Functions.OnCollisionExit);
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.GetComponent<ColliderAssertMarker>() != null && collision.gameObject.GetComponent<ColliderAssertMarker>().dependence == this)
                TryToCallOrder(Functions.OnCollisionStay);
        }
        //public void OnTriggerEnter2D()
        //{
        //    TryToCallOrder(Functions.OnTriggerEnter2D);
        //}
        //public void OnTriggerExit2D()
        //{
        //    TryToCallOrder(Functions.OnTriggerExit2D);
        //}
        //public void OnTriggerStay2D()
        //{
        //    TryToCallOrder(Functions.OnTriggerStay2D);
        //}
        //public void OnCollisionEnter2D()
        //{
        //    TryToCallOrder(Functions.OnCollisionEnter2D);
        //}
        //public void OnCollisionExit2D()
        //{
        //    TryToCallOrder(Functions.OnCollisionExit2D);
        //}
        //public void OnCollisionStay2D()
        //{
        //    TryToCallOrder(Functions.OnCollisionStay2D);
        //}
    }
}

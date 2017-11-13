﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using System.IO;

namespace SpaceCommander.Scenarios
{
    public class Mission1 : Scenario
    {
        public GameObject WarpGate1;
        public GameObject WarpGate2;
        public GameObject WarpGate3;
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {

        }
        public override int CheckVictory()
        {
            if (Scenario.DefaultOrder() == -1)
            {
                GetHelp();
                return 0;
            }
            else
                return base.CheckVictory();
        }

        private void GetHelp()
        {
            WarpGate1.GetComponent<WarpArrive>().Arrive();
            WarpGate2.GetComponent<WarpArrive>().Arrive();
            WarpGate3.GetComponent<WarpArrive>().Arrive();
        }
    }
}

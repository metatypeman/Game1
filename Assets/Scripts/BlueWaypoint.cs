﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlueWaypoint : BaseWayPoint
    {
        protected override void OnInitFacts()
        {
            base.OnInitFacts();

            this["color"] = "blue";
        }
    }
}

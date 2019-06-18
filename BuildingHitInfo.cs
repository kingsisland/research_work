using System.Collections;
using Wrld;
using Wrld.Space;
using Wrld.Resources.Buildings;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using CustomBuilding;
namespace CustomBuilding
{
    public class BuildingHitInfo
    {
        private int Hits { get; set; }
        private LatLong BuildingCentroid { get; set; }

        public BuildingHitInfo()
        {
            Hits = 0;
        }

       

    }

}

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


//Will  get the async part t work as intended  

namespace CustomBuilding.Procesing
{
    public  class GetBuildingInformationAndUpdateHits
    {
        private HashSet<StoreBuildingData> Buildings;

        private async Task< HashSet<StoreBuildingData>> Process (HashSet<Vector3> BuildingsList, Camera cam)  // attach an await statement
        {   
            foreach (var Point in BuildingsList)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(Point);
                Vector2 BuildingLocation = new Vector2(screenPos.x, screenPos.y);

               



            }
            // return
        }

        private void QueryApi (Vector2 Location)
        {
            var ray = Api.Instance.SpacesApi.ScreenPointToRay(Location);

            LatLongAltitude intersectionPoint;

            var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out intersectionPoint);

            if(didIntersectBuilding)
            {
                BuildingHighlight.Create(
                    new BuildingHighlightOptions()
                    .HighlightBuildingAtScreenPoint(Location)
                    .InformationOnly()
                    .BuildingInformationReceivedHandler(this.OnBuildingInformationReceived)
                    );
            }
        }

        void OnBuildingInformationReceived(BuildingHighlight highlight)
        {
            if(highlight.IsDiscarded())
            {
                Debug.Log(string.Format("No building information was received"));
                return;
            }

            var BuildingInformation = highlight.GetBuildingInformation();

            StoreBuildingData data = new StoreBuildingData();

            data.BuildingId = BuildingInformation.BuildingId;
            data.BuildingLocation = BuildingInformation.BuildingDimensions.Centroid;

            //  // use an arrayList to contain the objects of hit information & LAtlong location of a building


        }



    }
}

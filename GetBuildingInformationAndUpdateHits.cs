﻿using System.Collections;
using Wrld;
using Wrld.Space;
using Wrld.Resources.Buildings;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CustomBuilding;
using System.Collections.Concurrent;

//Will  get the async part t work as intended  

namespace CustomBuilding.Procesing
{
    public class GetBuildingInformationAndUpdateHits : MonoBehaviour
    {


        private ConcurrentDictionary<LatLong, int> BuildingStats;

        public float HighlightOnScreenTime;
        //  private bool IsLocked;

        public GetBuildingInformationAndUpdateHits()
        {
            BuildingStats = new ConcurrentDictionary<LatLong, int>();
            HighlightOnScreenTime = 5f;

        }

        public /*async Task*/ void Process(HashSet<Vector3> BuildingsList, Camera cam)  // attach an await statement
        {

            // List<Task> tasks = new List<Task> ();
            foreach (var Point in BuildingsList)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(Point);

                //tasks.Add(QueryApi(BuildingLocation));


                QueryApi(screenPos);


            }

            // await Task.WhenAll(tasks);



        }

        private /*async Task*/ void QueryApi(Vector3 Location)
        {
            var ray = Api.Instance.SpacesApi.ScreenPointToRay(Location);


            var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out LatLongAltitude intersectionPoint);

            if (didIntersectBuilding)
            {

                //  Debug.Log("didIntersectBuilding at: " + intersectionPoint);

                /* await Task.Run(() => BuildingHighlight.Create(
               new BuildingHighlightOptions()
               .HighlightBuildingAtScreenPoint(Location)
               .InformationOnly()
               .BuildingInformationReceivedHandler(this.OnBuildingInformationReceived)
               ));*/


                var intersectionPointLatLong = intersectionPoint.GetLatLong();


                BuildingHighlight highlight = BuildingHighlight.Create(
               new BuildingHighlightOptions()
               .HighlightBuildingAtScreenPoint(Location)
               .Color(new Color(1, 1, 0, 0.5f))
               /*.InformationOnly()*/
               .BuildingInformationReceivedHandler(this.OnBuildingInformationReceived)
               );







            }
        }

        void OnBuildingInformationReceived(BuildingHighlight highlight)
        {

            if (highlight.IsDiscarded())
            {
                Debug.Log(string.Format("No building information was received"));
                return;
            }





            var buildingInformation = highlight.GetBuildingInformation();



            StoreBuildingData data = new StoreBuildingData();

            data.BuildingId = buildingInformation.BuildingId;
            data.BuildingLocation = buildingInformation.BuildingDimensions.Centroid;

            BuildingStats.AddOrUpdate(data.BuildingLocation, 1, (key, oldValue) => oldValue + 1);

            // StartCoroutine(ClearHighlight(highlight));




        }

        public ConcurrentDictionary<LatLong, int> GetStats()
        {
            return BuildingStats;


        }


        IEnumerator ClearHighlight(BuildingHighlight highlight)
        {

            yield return new WaitForSeconds(HighlightOnScreenTime);

            Debug.Log("ClearHighlighs: ");
            highlight.Discard();
        }


    }
}

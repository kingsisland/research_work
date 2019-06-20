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
using CustomBuilding;
using System.Collections.Concurrent;

//Will  get the async part t work as intended  

namespace CustomBuilding.Procesing
{
    public class GetBuildingInformationAndUpdateHits: MonoBehaviour
    {


        private ConcurrentDictionary<LatLong, int> BuildingStats;    // A thread-safe dictionary to collect the building details

        public float HighlightOnScreenTime;    //defines the time for which the highlights stay on the game screen

        private int TotalBuildingsFound;
        private int TotalBuildingsInformationReceived;
      //  private bool IsLocked;

        public GetBuildingInformationAndUpdateHits()
        {
            BuildingStats = new ConcurrentDictionary<LatLong, int>();
            HighlightOnScreenTime = 5f;
            TotalBuildingsFound = 0;
            TotalBuildingsInformationReceived = 0;

        
        }

        public /*async Task*/ void  Process (HashSet<Vector3> BuildingsList, Camera cam )  // attach an await statement
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

        private /*async Task*/ void QueryApi (Vector3 Location)
        {
            var ray = Api.Instance.SpacesApi.ScreenPointToRay(Location);


            var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out LatLongAltitude intersectionPoint);

            if (didIntersectBuilding)
            {

                TotalBuildingsFound += 1;    //Keeps count of buildings hit (not unique) 

                  //var intersectionPointLatLong = intersectionPoint.GetLatLong();


                   BuildingHighlight highlight =  BuildingHighlight.Create(
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
       
            if(highlight.HasPopulatedBuildingInformation())
            {
                Debug.Log("HasPopulatedBuildingInformation: " + highlight.HasPopulatedBuildingInformation());
                TotalBuildingsInformationReceived += 1;   // //Keeps count of buildings whose info is received (not unique)
                Debug.Log("TotalBuildingsInformationReceived: " + TotalBuildingsInformationReceived);

            }

         

            BuildingStats.AddOrUpdate(buildingInformation.BuildingDimensions.Centroid , 1, (key, oldValue) => oldValue + 1);

            //StartCoroutine(ClearHighlight(highlight));

        }

        public ConcurrentDictionary<LatLong, int> GetStats ()
        {
             return BuildingStats;
            

        }


        IEnumerator ClearHighlight(BuildingHighlight highlight)   //clears highlights
        {
          
            yield return new WaitForSeconds(HighlightOnScreenTime);

            Debug.Log("ClearHighlighs: ");
            highlight.Discard();
        }

        /*public bool IsProcessingDone()
        {
            if (TotalBuildingsFound == TotalBuildingsInformationReceived)
          
                return true;
            else
            {
                Debug.Log("TotalBuildingsFound: " + TotalBuildingsFound + "     TotalBuildingsInformationReceived: " + TotalBuildingsInformationReceived);
            }
                return false;
        }*/


    }
}

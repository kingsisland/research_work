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
    public class GetBuildingInformationAndUpdateHits
    {


        private ConcurrentDictionary<LatLong, int> BuildingStats; 
      //  private bool IsLocked;

        public GetBuildingInformationAndUpdateHits()
        {
            BuildingStats = new ConcurrentDictionary<LatLong, int>();
        
        }

        public /*async Task*/ void  Process (HashSet<Vector3> BuildingsList, Camera cam )  // attach an await statement
        {
         
           // List<Task> tasks = new List<Task> ();
            foreach (var Point in BuildingsList)
            {
                   Vector3 screenPos = cam.WorldToScreenPoint(Point);
                   // Vector2 BuildingLocation = new Vector2(screenPos.x, screenPos.y);
                    //tasks.Add(QueryApi(BuildingLocation));

               
                    QueryApi(screenPos);
                
              
            }

             // await Task.WhenAll(tasks);
          

            Debug.Log("Api calls finished sucesfully");
          
            
            
        }

        private /*async Task*/ void QueryApi (Vector3 Location)
        {
            var ray = Api.Instance.SpacesApi.ScreenPointToRay(Location);


            var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out LatLongAltitude intersectionPoint);

            if (didIntersectBuilding)
            {
                
                Debug.Log("didIntersectBuilding at: " + intersectionPoint);
                
                    /* await Task.Run(() => BuildingHighlight.Create(
                   new BuildingHighlightOptions()
                   .HighlightBuildingAtScreenPoint(Location)
                   .InformationOnly()
                   .BuildingInformationReceivedHandler(this.OnBuildingInformationReceived)
                   ));*/

                    Debug.Log(intersectionPoint.GetLatitude() + "       " + intersectionPoint.GetLongitude());
                    var intersectionPointLatLong = intersectionPoint.GetLatLong();


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

            if(highlight.IsDiscarded())
            {
                Debug.Log(string.Format("No building information was received"));
                return;
            }



            Debug.Log("OnBuildingInformationReceived");

           /*   
                var buildingInformation = highlight.GetBuildingInformation();
                Debug.Log(highlight.HasPopulatedBuildingInformation());

                StoreBuildingData data = new StoreBuildingData();

                data.BuildingId = buildingInformation.BuildingId;
                    data.BuildingLocation = buildingInformation.BuildingDimensions.Centroid;
                
                
                    Debug.Log(data.BuildingId +  "      " + data.BuildingLocation);
                

                BuildingStats.AddOrUpdate(data.BuildingLocation, 1, (key, oldValue) => oldValue + 1);

                highlight.Discard();

            
          
*/




        }

        public ConcurrentDictionary<LatLong, int> GetStats ()
        {
             return BuildingStats;
            

        }



    }
}


using Wrld;
using Wrld.Space;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Windows;
using CustomBuilding;
using System.Collections.Concurrent;
using CustomBuilding.Procesing;
using System.Collections;
public class TestScript : MonoBehaviour
{
    public float radius;
    
    public GameObject sphere;

    public float partsXZ ;
    public float partsInY ;
    private float x, y, z;
    public bool allSet ;

    
    public float ConeAngle ;
    public float ConeSpread ;

    public float RayDurationTime;

    private HashSet<Vector3> BuildingsList;
    private HashSet<StoreBuildingData> Buildings;
    public  Camera cam;

    private ConcurrentDictionary<LatLong, int> BuildingStats;

    private void OnEnable()
    {
       /* var cameraLocation = LatLong.FromDegrees(36.0918, -115.1739);
        Api.Instance.CameraApi.MoveTo(cameraLocation, distanceFromInterest: 400, headingDegrees: 0, tiltDegrees: 45 );*/
    }

    private void Awake()
    {
        allSet = false;
        BuildingsList = new HashSet<Vector3>();
        Buildings = new HashSet<StoreBuildingData>();
       // BuildingStats = new ConcurrentDictionary<LatLong, int>();
       // cam = GetComponent<Camera>();
    }
  

      void Update()
    {   
        if(allSet == true)
        {
            // JuiceUp();
            StartCoroutine(JuiceUp());
             allSet = false;
        }
        
    }


 



    private /* async*/ IEnumerator JuiceUp()
    {
        
        float incrementTheta = ConeSpread / partsXZ;
        float incrementGamma = (90f - ConeAngle ) / partsInY;

        float r = 1f;
        float ThetaInDegrees = 0f;
        for( ; ThetaInDegrees <= ConeSpread; ThetaInDegrees += incrementTheta )
        {   
           
            float GammaInDegrees = 90f;

            for (; GammaInDegrees >= ConeAngle; GammaInDegrees -= incrementGamma )
            {
                x = r * Mathf.Sin(GammaInDegrees * Mathf.Deg2Rad) * Mathf.Cos(ThetaInDegrees * Mathf.Deg2Rad);
                y = r * Mathf.Cos( GammaInDegrees * Mathf.Deg2Rad);
                z = r * Mathf.Sin(GammaInDegrees * Mathf.Deg2Rad) * Mathf.Sin(ThetaInDegrees * Mathf.Deg2Rad);

                FireSomeRays(x, y, z);           // Fires Rays in all directions above ground
                FireSomeRays(-x, y, z);
                FireSomeRays(x, y, -z);
                FireSomeRays(-x, y, -z);

                
            }

        }

        // GetBuildingInformationAndUpdateHits getBuildingInfo= new GetBuildingInformationAndUpdateHits();
        GetBuildingInformationAndUpdateHits getBuildingInfo = GetComponent<GetBuildingInformationAndUpdateHits>();
       
             /*await*/ getBuildingInfo.Process(BuildingsList, cam);


        //Waiting for five seconds for the API to finish Loading its stuff

        Debug.Log("Time coroutine starts: " + System.DateTime.Now);
        yield return new WaitForSeconds(15);               //make this a variable for now
        Debug.Log("Time coroutine ends: " + System.DateTime.Now);




        BuildingStats = getBuildingInfo.GetStats();

        var VisualizationScript = GetComponent<Visualization>();
        VisualizationScript.FindOptimalBuildings(BuildingStats);

        

       

    }

    private void FireSomeRays(float x, float y, float z)    
    {
        RaycastHit hit;
      
        if (Physics.Raycast(sphere.transform.position,new Vector3(x, y, z), out hit, radius))
        {
            
            Debug.DrawRay(sphere.transform.position, -sphere.transform.position + hit.point, Color.yellow,60f);  
            AddBuildingToList(hit);
        }
       else
            Debug.DrawRay(sphere.transform.position, new Vector3(x, y, z) * radius, Color.red, RayDurationTime);


    }

   

    private void AddBuildingToList( RaycastHit hit)    //Get the hit information and adds it to a HashSet
    {
         BuildingsList.Add(hit.point);
    }


   /* IEnumerator WaitForAPI()
    {
        Debug.Log("Time coroutine starts: " + System.DateTime.Now);
         yield return new WaitForSeconds(5);
        Debug.Log("Time coroutine ends: " + System.DateTime.Now);
    }*/
    
    

}

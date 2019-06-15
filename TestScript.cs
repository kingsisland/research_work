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
    private  Camera cam;



    private void OnEnable()
    {
        var cameraLocation = LatLong.FromDegrees(36.0918, -115.1739);
        Api.Instance.CameraApi.MoveTo(cameraLocation, distanceFromInterest: 400, headingDegrees: 0, tiltDegrees: 45 );
    }

    private void Awake()
    {
        allSet = false;
        BuildingsList = new HashSet<Vector3>();
        Buildings = new HashSet<StoreBuildingData>();
        cam = GetComponent<Camera>();
    }
  

     void Update()
    {   
        if(allSet == true)
        {
            

            JuiceUp();
             allSet = false;
        }
        
    }


 



    private void JuiceUp()
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

        /*foreach (var item in BuildingsList)
        {
            Debug.Log(item);
        }*/
    }

    private void FireSomeRays(float x, float y, float z)    
    {
        RaycastHit hit;
      
        if (Physics.Raycast(sphere.transform.position,new Vector3(x, y, z), out hit, radius))
        {
            
            Debug.DrawRay(sphere.transform.position, -sphere.transform.position + hit.point, Color.yellow, 60f);
            AddBuildingToList(hit);
        }
       else
            Debug.DrawRay(sphere.transform.position, new Vector3(x, y, z) * radius, Color.red, RayDurationTime);


    }

   

    private void AddBuildingToList( RaycastHit hit)    //Get the hit information and adds it to a HashSet
    {
         BuildingsList.Add(hit.point);
    }



    
    

}

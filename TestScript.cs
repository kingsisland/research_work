using System.Collections;
using Wrld;
using Wrld.Space;
using Wrld.Resources.Buildings;
using UnityEngine;
using System;
using System.Collections.Generic;

public class TestScript : MonoBehaviour
{
    public float radius;
    
    public GameObject sphere;

    public float partsXZ = 5f;
    public float partsInY = 5f;
    private float x, y, z;
    public bool allSet ;
    private  HashSet<Vector3> BuildingsList = new HashSet<Vector3>();
    public float ConeAngle = 60f;
    public float ConeSpread = 90f;
    public float RayDurationTime = 10f;

 

    void Start()
    {
        allSet = false;
        
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

       /* Debug.Log(incrementTheta);
        Debug.Log(incrementGamma);*/

        float r = 1f;
        float ThetaInDegrees = 0f;
        for( ; ThetaInDegrees <= ConeSpread; ThetaInDegrees += incrementTheta )
        {   
           
            float GammaInDegrees = 90f;

             Debug.Log("Theta: " + ThetaInDegrees);
            

           for (; GammaInDegrees >= ConeAngle; GammaInDegrees -= incrementTheta )
            {
               


                /*Debug.Log("Gamma : " + GammaInDegrees);
                Debug.Log("ConeAngle : " + ConeAngle);*/

                x = r * Mathf.Sin(GammaInDegrees * Mathf.Deg2Rad) * Mathf.Cos(ThetaInDegrees * Mathf.Deg2Rad);
                y = r * Mathf.Cos( GammaInDegrees * Mathf.Deg2Rad);
                z = r * Mathf.Sin(GammaInDegrees * Mathf.Deg2Rad) * Mathf.Sin(ThetaInDegrees * Mathf.Deg2Rad);

                FireSomeRays(x, y, z);           // Fires Rays in all directions above ground
                FireSomeRays(-x, y, z);
                FireSomeRays(x, y, -z);
                FireSomeRays(-x, y, -z);

                
            }

            

        }

        foreach (var item in BuildingsList)
        {
            Debug.Log(item.x + "  " + item.y + "  " + item.z);
        }

        
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

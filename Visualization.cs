using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Wrld.Space;
using System.Collections.Concurrent;

public class Visualization : MonoBehaviour
{
    private List<KeyValuePair<LatLong, int>> BuildingsList = null;

   
    public void FindOptimalBuildings(ConcurrentDictionary<LatLong, int> BuildingStats)
    {
        Debug.Log("Count: " + BuildingStats.Count);

        var BuildingsArray = BuildingStats.ToArray();
        BuildingsList = new List<KeyValuePair<LatLong, int>>(BuildingsArray);

        //freeing some memory
        BuildingsArray = null;

        BuildingsList.Sort((x, y) => (y.Value).CompareTo(x.Value));      //Sorting the buildings on frequency basis

        foreach (var item in BuildingsList)
        {
            Debug.Log((item.Key.GetLatitude()).ToString() + "   " + (item.Key.GetLongitude()).ToString() + "      " + item.Value);
        }

    }

     public List<KeyValuePair<LatLong, int>> GetSortedList ()
     {
          return BuildingsList;
     }
}

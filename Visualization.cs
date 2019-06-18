using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Wrld.Space;
using System.Collections.Concurrent;

public class Visualization : MonoBehaviour
{
   public void FindOptimalBuildings(ConcurrentDictionary<LatLong, int> BuildingStats)
    {
        var BuildingsArray = BuildingStats.ToArray();
        var BuildingsList = new List<KeyValuePair<LatLong, int>>(BuildingsArray);

        //freeing some memory
        BuildingsArray = null;

        BuildingsList.Sort((x, y) => (y.Value).CompareTo(x.Value));

        foreach(var item in BuildingsList)
        {
            Debug.Log((item.Key.GetLatitude()).ToString() + "   " + (item.Key.GetLongitude()).ToString() + "      " + item.Value);
        }


    }
}

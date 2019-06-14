using System.Collections;
using Wrld;
using Wrld.Space;
using Wrld.Resources.Buildings;
using UnityEngine;

public class CollectBuildings: MonoBehaviour
{

    public float radius;
    public Vector3 centre;
    private LatLong buildingLocation;
    public GameObject sphere;
    public GameObject square;
    public float trans;
    private Color Color;
    void Start()
    {     
     
       
        Color = sphere.GetComponent<Renderer>().material.color;

     




    }
    private void Update()
    {
        Color.a = trans;
        sphere.transform.position = square.transform.position;
        sphere.transform.localScale = new Vector3(radius, radius, radius);
        //sphere.transform.localScale = new Vector3(radius, radius, radius);
       

    }

    //private void OnEnable()
    //{
    //    StartCoroutine(Example());
    //}

    

    //IEnumerator Example()
    //{
    //    Api.Instance.CameraApi.MoveTo(buildingLocation, distanceFromInterest: 500, headingDegrees: 0, tiltDegrees: 45);

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(4.0f);

    //        BuildingHighlight.Create(
    //            new BuildingHighlightOptions()
    //                .HighlightBuildingAtLocation(buildingLocation)
    //                .BuildingInformationReceivedHandler(this.OnBuildingInformationReceived)
    //                .InformationOnly()
    //        );
    //    }
    //}

    //void OnBuildingInformationReceived(BuildingHighlight highlight)
    //{
    //    if (highlight.IsDiscarded())
    //    {
    //        Debug.Log(string.Format("No building information was received"));
    //        return;
    //    }

    //    var buildingInformation = highlight.GetBuildingInformation();

    //    var boxAnchor = Instantiate(boxPrefab) as GameObject;
    //    boxAnchor.GetComponent<GeographicTransform>().SetPosition(buildingInformation.BuildingDimensions.Centroid);

    //    var box = boxAnchor.transform.GetChild(0);
    //    box.localPosition = Vector3.up * (float)buildingInformation.BuildingDimensions.TopAltitude;
    //    Destroy(boxAnchor, 2.0f);

    //    Debug.Log(string.Format("Building information received: {0}", buildingInformation.ToJson()));

    //    highlight.Discard();
    //}

    //private void OnDisable()
    //{
    //    StopAllCoroutines();
    //}
}
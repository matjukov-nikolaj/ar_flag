using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToSetController : MonoBehaviour
{
    public GameObject flag;
    
    public GameObject placementIndicator;
    
    private ARSessionOrigin arOrigin;

    private ARRaycastManager arManager; 

    private Pose placementPosition;

    private bool placementPosIsValid;
    
    void Start()
    {
        flag = GameObject.Find("Flag");
        placementIndicator = GameObject.Find("Placement Indicator");
        placementPosIsValid = false;
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    void Update()
    {
        UpdatePlacementPosition();
        UpdatePlacementIndicator();

        if (placementPosIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceFlag();

        }
    }

    private void PlaceFlag()
    {
        Instantiate(flag, placementPosition.position, placementPosition.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPosIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPosition.position, placementPosition.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPosition()
    {
        arManager = FindObjectOfType<ARRaycastManager>();
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPosIsValid = hits.Count > 0;
        if (placementPosIsValid)
        {
            placementPosition = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.y).normalized;
            placementPosition.rotation = Quaternion.LookRotation(cameraBearing);
            
        }
    }
}

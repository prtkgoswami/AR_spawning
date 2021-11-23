using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARPlacement : MonoBehaviour
{
    public GameObject[] spawnPrefab;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool validPlacementPose = false;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject spawnedObject;
    private int currObjectIndex = 0;
    private bool objectChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = GetComponent<ARSessionOrigin>();
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdatePlacementPosition();

        if (!CheckTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = hits[0].pose;
            SpawnObject(hitPos);
        }

    }

    private bool CheckTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void SpawnObject(Pose spawnPose)
    {
        Vector3 cameraForward = Camera.current.transform.forward;
        Vector3 cameraBearing = -new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        if (spawnedObject == null || objectChanged)
        {
            Destroy(spawnedObject); 
            spawnedObject = Instantiate(spawnPrefab[currObjectIndex], spawnPose.position, Quaternion.LookRotation(cameraBearing));
            objectChanged = false;
        }
        else
        {
            spawnedObject.transform.position = spawnPose.position;
            spawnedObject.transform.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementPosition()
    {
        Vector2 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(.5f, 0, .5f));

        arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        validPlacementPose = hits.Count > 0;
        if (validPlacementPose)
        {
            placementPose = hits[0].pose;
            Vector3 cameraForward = Camera.current.transform.forward;
            Vector3 cameraBearing = -new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    public void ChangeImagePrefab(int index)
    {
        currObjectIndex = index;
        objectChanged = true;
    }
}

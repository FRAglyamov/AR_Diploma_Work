using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


//[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{

    public GameObject gameObjectToInstanstiate;

    private GameObject _spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        //_arRaycastManager = GetComponent<ARRaycastManager>();
        _arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if(_spawnedObject == null)
            {
                _spawnedObject = Instantiate(gameObjectToInstanstiate, hitPose.position, hitPose.rotation);
            }
            else
            {
                _spawnedObject.transform.position = hitPose.position;
            }
        }
    }
}

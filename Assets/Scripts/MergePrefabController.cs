using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePrefabController : MonoBehaviour
{
    
    [SerializeField] List<GameObject> _listOfPrefabs = new List<GameObject>();
    private GameObject _currentRayCastObject;
    private Vector3 _currentPrefabPosition;
    [SerializeField] LayerMask _layer;
    private void OnMouseDown()
    {
        _currentPrefabPosition = transform.position; // get the current position of the fighter and set the variable to that value 
        gameObject.layer = 0;   
    }
    private void OnMouseDrag()
    {

    }
    private void OnMouseUp()
    {

    }
}

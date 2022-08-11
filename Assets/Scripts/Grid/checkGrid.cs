using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkGrid : MonoBehaviour
{
    public bool _onGrid;
  
    void Start()
    {
        
    }

    void Update()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Grid")
            {
                
                _onGrid = true;

            }
            else
            {
                _onGrid = false;
            }
        }
    }
}

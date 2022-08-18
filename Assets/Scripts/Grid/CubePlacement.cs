using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlacement : MonoBehaviour
{
    public GameObject ObjectToMove;
    public GameObject ObjectToPlace;
    public LayerMask Mask;
    public float LastPositionY;
    public Vector3 MousePosition;
    private Renderer Render;
    public Material GridMaterial, DefaultMaterial;

    private Touch theTouch;

    
    void Start()
    {
        Render = GameObject.Find("Grid").GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
                theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(theTouch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask))
                {
                    int PosX = (int)Mathf.Round(hit.point.x);
                    int PosZ = (int)Mathf.Round(hit.point.z);
                    if (hit.transform.name == gameObject.name)
                    {
                        ObjectToMove.transform.position = new Vector3(PosX, LastPositionY, PosZ);
                        Render.material = GridMaterial;
                    }

                }
            }
        }


        if (theTouch.phase == TouchPhase.Ended)
        {
            Instantiate(ObjectToPlace, ObjectToMove.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Render.material = DefaultMaterial;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject ObjectToPlace;
    public LayerMask Mask;
    public float LastPositionY;
    public Vector3 MousePosition;
    private Renderer Render;
    public Material GridMaterial, DefaultMaterial;
    private bool _isDraging;


    void Start()
    {
        Render = GameObject.Find("Grid").GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _isDraging = true;
            MousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask))
            {
                int PosX = (int)Mathf.Round(hit.point.x);
                int PosZ = (int)Mathf.Round(hit.point.z);

                ObjectToPlace.transform.position = new Vector3(PosX, LastPositionY, PosZ);
            }
        }
        else
        {
            _isDraging = false;
        }


        if (_isDraging)
        {
            Render.material = GridMaterial;
        }
        else if (!_isDraging)
        {
            Render.material = DefaultMaterial;
        }
    }
}

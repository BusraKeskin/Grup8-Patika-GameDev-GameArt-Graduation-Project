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

    void Start()
    {
        Render = GameObject.Find("Grid").GetComponent<Renderer>();
    }

    void Update()
    {
        MousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(MousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            ObjectToMove.transform.position = new Vector3(PosX, LastPositionY, PosZ);
            Render.material = GridMaterial;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(ObjectToPlace, ObjectToMove.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Render.material = DefaultMaterial;
        }
    }
}

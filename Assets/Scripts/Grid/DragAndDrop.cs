using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    //public GameObject ObjectToPlace;
    //public LayerMask Mask;
    public float LastPositionY; //Tasinan objenin y pozisyonunun ne olacagini belirtir (surekli sabit 0.5)
    public Vector3 MousePosition;
    private Renderer Render; //Zemin renderi - suruklerken gridleri gosterip kapatmak icin kullanılıyor
    public Material GridMaterial, DefaultMaterial;
    private bool _isDraging;
    private bool _isMe;


    void Start()
    {
        Render = GameObject.Find("Grid").GetComponent<Renderer>(); //Grid zemin
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Mouse a tıklandiginda
        {

            MousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) //Eger mouse un bulundugu yerde bir obje varsa
            {
                int PosX = (int)Mathf.Round(hit.point.x); //Grid hareketi vermek için koordinatları tam sayiya yuvarliyoruz
                int PosZ = (int)Mathf.Round(hit.point.z);
                if (hit.transform.name == gameObject.name) //Mouse un tiklandıgı obje bu scriptin atandigi obje ise,
                {
                    _isMe = true;
                }
                else
                {
                    _isMe = false;
                }

            }

        }
        else if (Input.GetMouseButton(0)) //Mouse basılı durumdayken,
        { 
            _isDraging = true;
        }
        else if (Input.GetMouseButtonUp(0))  //Elimi mousetan kaldirdigimda,
        { 
            if(_isMe) //Az once tiklanan obje bensem
            {
                _isDraging = false;
                _isMe = false;
                Render.material = DefaultMaterial;
            }
          
        } 
        if (_isDraging && _isMe) //Eger surukleme durumundaysa ve suruklenen obje scriptin oldugu obje ise,
        {
            MousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                int PosX = (int)Mathf.Round(hit.point.x);
                int PosZ = (int)Mathf.Round(hit.point.z);
                
                    transform.position = new Vector3(PosX, LastPositionY, PosZ); //Bu objeyi hareket ettir
                 
            }
            Render.material = GridMaterial; // Zemin materyalini grid materyale çevir
        }    
    }
}

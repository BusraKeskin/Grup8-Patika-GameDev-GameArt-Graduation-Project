using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    public GameObject objectToMove, objectTMInstance, gridMark, instance; //surukleme esnasında gorunecek olan obje
    public float LastPositionY; //Tasinan objenin y pozisyonunun ne olacagini belirtir (surekli sabit 0.5)
    public Vector3 MousePosition;
    //private Renderer Render; //Zemin renderi - suruklerken gridleri gosterip kapatmak icin kullanılıyor
    public Material GridMaterial, DefaultMaterial;
    private bool _isDraging;
    private bool _isMe;
    private bool _checkMerge;
    private LayerMask ElonMask;
    private LayerMask AllMask;
    private LayerMask GridMask;
    private Transform firstGrid;
    private bool overClick = false;

    public DragAndDropScriptable DragAndDropValues;


    private bool _onGrid;

    private Touch theTouch;

    void Start()
    {
        Physics.IgnoreCollision(instance.GetComponent<Collider>(), transform.GetComponent<Collider>());
        //Render = GameObject.Find("Grid").GetComponent<Renderer>(); //Grid zemin
        ElonMask = DragAndDropValues.AllMask;
    }

    void Update()
    {
        if (!GameManager.Instance._isStart && !overClick)
        {
            if (Input.touchCount > 0)
            {
                theTouch = Input.GetTouch(0);
                if (theTouch.phase == TouchPhase.Began) //Mouse a tıklandiginda
                {
                    Ray ray = Camera.main.ScreenPointToRay(theTouch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ElonMask)) //Eger mouse un bulundugu yerde bir obje varsa
                    {
                        //int PosX = (int)Mathf.Round(hit.point.x); //Grid hareketi vermek için koordinatları tam sayiya yuvarliyoruz
                        //int PosZ = (int)Mathf.Round(hit.point.z);
                        if (hit.transform.GetInstanceID() == gameObject.transform.GetInstanceID()) //Mouse un tiklandıgı obje bu scriptin atandigi obje ise,
                        {

                            _isMe = true;
                            RaycastHit _hit;
                            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit, 10f, DragAndDropValues.GridMask))
                            {
                                firstGrid = _hit.transform;
                                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow, 5);
                                //LevelManager.Instance.findGrid(_hit.transform);
                                LevelManager.Instance.setGridEmptyState(firstGrid, true);
                            }
                            instance.SetActive(false);
                            objectTMInstance = GameObject.Instantiate(objectToMove, transform.position, Quaternion.identity);
                        }
                        else
                        {
                            _isMe = false;
                        }

                    }

                }
                else if (theTouch.phase == TouchPhase.Moved) //Mouse basılı durumdayken,
                {
                    _isDraging = true;
                    _checkMerge = false;

                }
                else if (theTouch.phase == TouchPhase.Ended)  //Elimi mousetan kaldirdigimda,
                {
                    overClick = true;
                    StartCoroutine("preventOverClick");
                    if (_isMe) //Az once tiklanan obje bensem
                    {
                        Ray __ray = Camera.main.ScreenPointToRay(theTouch.position);
                        RaycastHit __hit;

                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out __hit, 10f, DragAndDropValues.GridMask))
                        {
                            if (LevelManager.Instance.checkIfGridEmpty(__hit.transform))
                            {
                                LevelManager.Instance.setGridEmptyState(__hit.transform, false);
                            }
                        }

                        _isDraging = false;
                        _isMe = false;
                        _checkMerge = true;
                        instance.SetActive(true);
                        gridMark.SetActive(false);
                        Destroy(objectTMInstance);
                        ElonMask = DragAndDropValues.AllMask;
                    }

                }
            }
            if (_isDraging && _isMe) //Eger surukleme durumundaysa ve suruklenen obje scriptin oldugu obje ise,
            {
                ElonMask = DragAndDropValues.GridMask;
                theTouch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(theTouch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ElonMask))
                {
                    //int PosX = (int)Mathf.Round(hit.point.x);
                    //int PosZ = (int)Mathf.Round(hit.point.z);
                    int PosX = (int)Mathf.Round(hit.transform.GetComponent<Renderer>().bounds.center.x);
                    int PosZ = (int)Mathf.Round(hit.transform.GetComponent<Renderer>().bounds.center.z);

                    gridMark.SetActive(true);
                    transform.position = new Vector3(PosX, transform.position.y, PosZ); //Bu objeyi hareket ettir
                    objectTMInstance.transform.position = Vector3.MoveTowards(objectTMInstance.transform.position, new Vector3(hit.point.x, LastPositionY, hit.point.z), 40 * Time.deltaTime);
                }
                //Render.material = GridMaterial; // Zemin materyalini grid materyale çevir
            }
        }

    }


    IEnumerator preventOverClick()
    {
        yield return new WaitForSeconds(0.1f);
        overClick = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (_checkMerge)
        {
            Fighter.Type myType = gameObject.GetComponent<Fighter>().type;

            if (other.gameObject.GetComponent<Fighter>()?.type == myType)
            {
                if (myType == Fighter.Type.Wizard_v1)
                {

                    GameObject.Instantiate(Resources.Load("Prefabs/Wizard_v2"), transform.position, Quaternion.identity);

                }
                else if (myType == Fighter.Type.MeleeFighter_v1)
                {


                    GameObject.Instantiate(Resources.Load("Prefabs/MeleeFighter_v2"), transform.position, Quaternion.identity);

                }
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            else
            {
                LevelManager.Instance.setGridEmptyState(firstGrid, false);
                Vector3 pos = firstGrid.GetComponent<Renderer>().bounds.center;
                gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
            }

        }
    }
}
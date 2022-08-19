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
            if (Input.GetMouseButtonDown(0)) //Mouse a tıklandiginda
            {
                MousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(MousePosition);
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
            else if (Input.GetMouseButton(0)) //Mouse basılı durumdayken,
            {
                _isDraging = true;
                _checkMerge = false;

            }
            else if (Input.GetMouseButtonUp(0))  //Elimi mousetan kaldirdigimda,
            {
                overClick = true;
                StartCoroutine("preventOverClick");
                if (_isMe) //Az once tiklanan obje bensem
                {
                    Ray __ray = Camera.main.ScreenPointToRay(MousePosition);
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
            if (_isDraging && _isMe) //Eger surukleme durumundaysa ve suruklenen obje scriptin oldugu obje ise,
            {
                ElonMask = DragAndDropValues.GridMask;
                MousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(MousePosition);
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
            CharacterSO.Type myType = gameObject.GetComponent<Fighter>().characterSO.type;
           
            if (other.gameObject.GetComponent<Fighter>()?.characterSO.type == myType)
            {
                
                if (myType == CharacterSO.Type.Wizard_v1)
                {
                    GameManager.Instance.updateHeroListAfterMerge("Wizard_v1", "Wizard_v2");
                    GameObject.Instantiate(Resources.Load("Prefabs/Wizard_v2"), transform.position, Quaternion.identity);
                   //gameObject.transform.parent.name == "Heroes";
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                else if (myType == CharacterSO.Type.MeleeFighter_v1)
                {

                    GameManager.Instance.updateHeroListAfterMerge("MeleeFighter_v1", "MeleeFighter_v2");
                    GameObject.Instantiate(Resources.Load("Prefabs/MeleeFighter_v2"), transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                else if (myType == CharacterSO.Type.MeleeFighter_v2)
                {

                    GameManager.Instance.updateHeroListAfterMerge("MeleeFighter_v2", "MeleeFighter_v3");
                    GameObject.Instantiate(Resources.Load("Prefabs/MeleeFighter_v3"), transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                else if (myType == CharacterSO.Type.Wizard_v2)
                {
                    GameManager.Instance.updateHeroListAfterMerge("Wizard_v2", "Wizard_v3");
                    GameObject.Instantiate(Resources.Load("Prefabs/Wizard_v3"), transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                else if (myType == CharacterSO.Type.Wizard_v3)
                {

                    LevelManager.Instance.setGridEmptyState(firstGrid, false);
                    Vector3 pos = firstGrid.GetComponent<Renderer>().bounds.center;
                    gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
                }
                else if (myType == CharacterSO.Type.MeleeFighter_v3)
                {

                    LevelManager.Instance.setGridEmptyState(firstGrid, false);
                    Vector3 pos = firstGrid.GetComponent<Renderer>().bounds.center;
                    gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
                }

                LevelManager.Instance.isAllGridFull = false;
                
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

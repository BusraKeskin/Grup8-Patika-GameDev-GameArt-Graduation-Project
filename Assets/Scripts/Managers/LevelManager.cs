using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    private Dictionary<string, Dictionary<string, object>> gridList = new Dictionary<string, Dictionary<string, object>>();
    private Transform GridLayout;
    [SerializeField] private GameObject HeroesParentObject;
    private Transform gridToPlaceHero;
    public bool isAllGridFull = false;
    public String[] heroes;
    private void Start()
    {
        heroes = GameManager.Instance.getHeroes();
        Debug.Log(heroes);
        GridLayout = GameObject.FindGameObjectWithTag("GridLayout").transform; //GridLayout(gridlerin parent i) u bul
        foreach (Transform child in GridLayout)//her bir gridde dön
        {
            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                { "isEmpty", true },//her grid baþlangýçta boþ lacak
                { "item", child}//dictionary içinde gridin Transform unu tutuyoruz
            };
            gridList.Add(child.name, d);
        }


        foreach (string heroName in heroes)
        {
            
            bool result = placeIfAvailable(heroName);
            
            if (!result)
            {
                Debug.Log("Grid full");
               
                break;
            }

        }

        //setGridEmptyState(GameObject.Find("stone_grid (2)").transform, false);
        //setGridEmptyState(GameObject.Find("stone_grid (2)").transform, false);
        //setGridEmptyState(GameObject.Find("stone_grid (13)").transform, false);


        //elimizde þöyle bir Dict oluþtu:

        // {
        //  "stone_grid": {
        //      "isEmpty": true,
        //      "item": [Transform]
        //  },
        //
        //  "stone_grid(1)": {
        //      "isEmpty": true,
        //      "item": [Transform]
        //  }
        // }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var data in gridList)
            {
                Transform item = (Transform)(data.Value["item"]);
                Debug.Log(item.name + "  " + data.Value["isEmpty"]);
            }
        }
    }

    public object getGridByName(string name)
    {
        foreach (var data in gridList)
        {
            if (data.Key == name)
            {
                return  data;
            }
        }

        return false;
    }

    public bool checkIfGridEmpty(Transform gridObject)
    {
        foreach (var data in gridList)
        {
            bool isEmpty = (bool)(data.Value["isEmpty"]);
            if (isEmpty)
            {
                Transform dataItem = (Transform)(data.Value["item"]);
                if (dataItem.name == gridObject.name)
                {
                    return true;
                }
            }
            
        }

        return false;
    }

    public bool setGridEmptyState(Transform gridObject ,bool state)
    {
        foreach (var data in gridList)
        {

            Transform dataItem = (Transform)(data.Value["item"]);
            if (dataItem.name == gridObject.name)
            {
                data.Value["isEmpty"] = (bool)state;
                //Debug.Log("Grid information has been changed!");
                return true;
            }

        }

        //Debug.Log("Grid information can not be changed!");
        return false;
    }

    public void GetAvailableGrid()
    {
        KeyValuePair<string, Dictionary<string, object>> gridAsListItem = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid"));
        if ((bool)gridAsListItem.Value["isEmpty"] == true)
        {
            gridToPlaceHero = (Transform)(gridAsListItem.Value["item"]);
        }
        else
        {
            KeyValuePair<string, Dictionary<string, object>> gridAsListItem_2 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (1)"));
            if ((bool)gridAsListItem_2.Value["isEmpty"] == true)
            {
                gridToPlaceHero = (Transform)(gridAsListItem_2.Value["item"]);
            }
            else
            {
                KeyValuePair<string, Dictionary<string, object>> gridAsListItem_3 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (2)"));
                if ((bool)gridAsListItem_3.Value["isEmpty"] == true)
                {
                    gridToPlaceHero = (Transform)(gridAsListItem_3.Value["item"]);
                }
                else
                {
                    KeyValuePair<string, Dictionary<string, object>> gridAsListItem_4 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (3)"));
                    if ((bool)gridAsListItem_4.Value["isEmpty"] == true)
                    {
                        gridToPlaceHero = (Transform)(gridAsListItem_4.Value["item"]);
                    }
                    else
                    {
                        KeyValuePair<string, Dictionary<string, object>> gridAsListItem_5 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (4)"));
                        if ((bool)gridAsListItem_5.Value["isEmpty"] == true)
                        {
                            gridToPlaceHero = (Transform)(gridAsListItem_5.Value["item"]);
                        }
                        else
                        {
                            KeyValuePair<string, Dictionary<string, object>> gridAsListItem_6 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (5)"));
                            if ((bool)gridAsListItem_6.Value["isEmpty"] == true)
                            {
                                gridToPlaceHero = (Transform)(gridAsListItem_6.Value["item"]);
                            }
                            else
                            {
                                KeyValuePair<string, Dictionary<string, object>> gridAsListItem_7 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (6)"));
                                if ((bool)gridAsListItem_7.Value["isEmpty"] == true)
                                {
                                    gridToPlaceHero = (Transform)(gridAsListItem_7.Value["item"]);
                                }
                                else
                                {
                                    KeyValuePair<string, Dictionary<string, object>> gridAsListItem_8 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (7)"));
                                    if ((bool)gridAsListItem_8.Value["isEmpty"] == true)
                                    {
                                        gridToPlaceHero = (Transform)(gridAsListItem_8.Value["item"]);
                                    }
                                    else
                                    {
                                        KeyValuePair<string, Dictionary<string, object>> gridAsListItem_9 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (8)"));
                                        if ((bool)gridAsListItem_9.Value["isEmpty"] == true)
                                        {
                                            gridToPlaceHero = (Transform)(gridAsListItem_9.Value["item"]);
                                        }
                                        else
                                        {
                                            KeyValuePair<string, Dictionary<string, object>> gridAsListItem_10 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (9)"));
                                            if ((bool)gridAsListItem_10.Value["isEmpty"] == true)
                                            {
                                                gridToPlaceHero = (Transform)(gridAsListItem_10.Value["item"]);
                                            }
                                            else
                                            {
                                                KeyValuePair<string, Dictionary<string, object>> gridAsListItem_11 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (10)"));
                                                if ((bool)gridAsListItem_11.Value["isEmpty"] == true)
                                                {
                                                    gridToPlaceHero = (Transform)(gridAsListItem_11.Value["item"]);
                                                }
                                                else
                                                {
                                                    KeyValuePair<string, Dictionary<string, object>> gridAsListItem_12 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (11)"));
                                                    if ((bool)gridAsListItem_12.Value["isEmpty"] == true)
                                                    {
                                                        gridToPlaceHero = (Transform)(gridAsListItem_12.Value["item"]);
                                                    }
                                                    else
                                                    {
                                                        KeyValuePair<string, Dictionary<string, object>> gridAsListItem_13 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (12)"));
                                                        if ((bool)gridAsListItem_13.Value["isEmpty"] == true)
                                                        {
                                                            gridToPlaceHero = (Transform)(gridAsListItem_13.Value["item"]);
                                                        }
                                                        else
                                                        {
                                                            KeyValuePair<string, Dictionary<string, object>> gridAsListItem_14 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (13)"));
                                                            if ((bool)gridAsListItem_14.Value["isEmpty"] == true)
                                                            {
                                                                gridToPlaceHero = (Transform)(gridAsListItem_14.Value["item"]);
                                                            }
                                                            else
                                                            {
                                                                KeyValuePair<string, Dictionary<string, object>> gridAsListItem_15 = (KeyValuePair<string, Dictionary<string, object>>)(getGridByName("stone_grid (14)"));
                                                                if ((bool)gridAsListItem_15.Value["isEmpty"] == true)
                                                                {
                                                                    gridToPlaceHero = (Transform)(gridAsListItem_15.Value["item"]);
                                                                }
                                                                else
                                                                {
                                                                    isAllGridFull = true;
                                                                    gridToPlaceHero = (Transform)(gridAsListItem_15.Value["item"]);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
     }

    public bool placeIfAvailable(string heroName)
    {

        GetAvailableGrid();
        
        if (isAllGridFull)
        {
            return false;

        }
        else
        {
            setGridEmptyState(gridToPlaceHero, false);
            Vector3 pos = gridToPlaceHero.GetComponent<Renderer>().bounds.center;
            //Debug.Log(heroName);
            GameObject newHero = (GameObject)(GameObject.Instantiate(Resources.Load("Prefabs/" + heroName), new Vector3(pos.x, 0.07f, pos.z), Quaternion.identity));
            newHero.transform.parent = HeroesParentObject.transform;
                
            return true;
        }
    }
    
}

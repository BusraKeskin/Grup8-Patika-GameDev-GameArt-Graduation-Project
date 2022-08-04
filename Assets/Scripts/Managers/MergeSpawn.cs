using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSpawn : MonoBehaviour
{
    [SerializeField] GameObject _prefabToSpawn;
    public List<GameObject> _listOfAvailableTiles = new List<GameObject>();
    private GameObject _tileToSpawnOn;

    void Start()
    {
        _listOfAvailableTiles.AddRange(GameObject.FindGameObjectsWithTag("Platform"));
        StartCoroutine(Spawn());
    }


    void Update()
    {



    }
    IEnumerator Spawn()
    {
        if (_listOfAvailableTiles.Count != 0)
        {
            _tileToSpawnOn = _listOfAvailableTiles[Random.Range(0, _listOfAvailableTiles.Count - 1)];
            GameObject _currentPrefab = Instantiate(_prefabToSpawn, new Vector3(_tileToSpawnOn.transform.position.x, _tileToSpawnOn.transform.position.y,0.1f), _tileToSpawnOn.transform.rotation);
            yield return new WaitForSeconds(3);
            _listOfAvailableTiles.Remove(_tileToSpawnOn);
            _tileToSpawnOn.layer = 0;
            
        }
        
    }
}
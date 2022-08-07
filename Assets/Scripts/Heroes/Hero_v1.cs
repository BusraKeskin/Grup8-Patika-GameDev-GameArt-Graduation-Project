using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hero_v1 : Fighter
{
    private bool _checkCol;
    private NavMeshAgent _agent;
    private GameObject[] _enemies;
    
        List<float> distances;
   
    void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _agent = GetComponent<NavMeshAgent>();
    }

    GameObject findNearestEnemy(){
        float distance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (var enemy in _enemies)
        {
            if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) < distance)
            {
                nearestEnemy = enemy;
                distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            }
        }
        return nearestEnemy;
    }

    void Update()
    {
        //_checkCol = transform.GetComponent<DragAndDrop>()._checkCol;
        
        _agent.SetDestination(findNearestEnemy().transform.position);
    }
}

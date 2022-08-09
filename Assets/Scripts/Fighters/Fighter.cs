using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fighter : MonoBehaviour
{
    public float _damage;
    public float _health;


    public enum  Type
    {
        MeleeFighter_v1,
        MeleeFighter_v2,
        MeleeFighter_v3,
        Wizard_v1,
        Wizard_v2,
        Wizard_v3

    }
    public Type type;
    private NavMeshAgent _agent;
    private GameObject[] _enemies, _heroes;

    List<float> distances;

    void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _heroes = GameObject.FindGameObjectsWithTag("Hero");
        _agent = GetComponent<NavMeshAgent>();
    }

    GameObject findNearestEnemy()
    {
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
    GameObject findNearestHero()
    {
        float distance = Mathf.Infinity;
        GameObject nearestHero = null;
        foreach (GameObject hero in _heroes)
        {
            if (Vector3.Distance(gameObject.transform.position, hero.transform.position) < distance)
            {
                nearestHero = hero;
                distance = Vector3.Distance(gameObject.transform.position, hero.transform.position);
            }
        }
        return nearestHero;
    }
    void Update()
    {
        Vector3 target = Vector3.zero;
        //_checkCol = transform.GetComponent<DragAndDrop>()._checkCol;
        if (gameObject.tag == "Hero") 
        { 
            target = findNearestEnemy().transform.position;
        }
        else
        {
            target = findNearestHero().transform.position;
        }
        //_agent.SetDestination(new Vector3(target.x, transform.position.y, target.z));
    }

}

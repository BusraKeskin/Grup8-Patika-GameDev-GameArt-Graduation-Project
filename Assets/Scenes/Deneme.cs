using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deneme : MonoBehaviour
{

    public GameObject instance;

    private void Start()
    {
        Physics.IgnoreCollision(instance.GetComponent<Collider>(), transform.GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}

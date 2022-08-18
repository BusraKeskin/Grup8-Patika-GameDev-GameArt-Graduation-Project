using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private Transform targetsPartToHit;
    private GameObject target;
    public float speed;
    public float damage;
    public GameObject impactEffect;

    private void Start()
    {
        speed = 1f;
    }
    void Update()
    {
        if(GameManager.Instance._isStart == false)
        {
            Destroy(gameObject);
        }
        if (targetsPartToHit == null)
        {
            Destroy(gameObject);
            return;
        }
        if(target)
        {
            Vector3 dir = targetsPartToHit.transform.position - transform.position;
            float speedModifier = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(transform.position, targetsPartToHit.transform.position, speedModifier);

            if (dir.magnitude <= speedModifier)
            {
                if(target)
                {
                    HitTarget();
                    return;
                }
            }
        }
    }

    public void Seek(Transform _targetPosition, GameObject lockedTarget, float attackDamage)
    {
        targetsPartToHit = _targetPosition;
        target = lockedTarget;
        damage = attackDamage;
    }

    void HitTarget()
    {
        if(target)
        {
            GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 1f);
            Destroy(gameObject);
            target.GetComponent<Fighter>().DealDamage(damage);
        }
    }
}

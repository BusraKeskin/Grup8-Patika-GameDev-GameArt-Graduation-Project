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
        speed = 2.5f;
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
        }
    }

    public void Seek(Transform _targetPosition, GameObject lockedTarget, float attackDamage)
    {
        targetsPartToHit = _targetPosition;
        target = lockedTarget;
        damage = attackDamage;
    }

    void DealSpellDamage(float damage)
    {
        target.GetComponent<Fighter>().CurrentHealth -= damage;

        if(target.tag == "Enemy")
        {
            GameManager.Instance.calculateCoin((int)(damage * GameManager.Instance.CoinMultiplier));
            GameManager.Instance.LevelCoin += (int)(damage * GameManager.Instance.CoinMultiplier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == target.tag)
        {
            if (target)
            {
                GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(effectIns, 1f);
                Destroy(gameObject);
                DealSpellDamage(damage);
            }
        }
    }
}

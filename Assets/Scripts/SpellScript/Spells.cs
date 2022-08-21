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

                if (target.tag == "Enemy")
                { //Eðer hasarý alan objenin tag i "Enemy" ise, yani hero hasar verdiyse
                    GameManager.Instance.coins += GameManager.Instance.spellHitReward; //Kur Korumalý Mevduattan(Büyücü hero düþmana hasar verdiðinde) 1 coin kazan
                    PlayerPrefs.SetInt("coins", GameManager.Instance.coins); //artýrýlmýþ sonucu hafýzaya kaydet
                    GameManager.Instance.coinsText.text = GameManager.Instance.coins.ToString(); // yeni coin deðerini ui da yazdýr
                }
            }
        }
        
    }
}

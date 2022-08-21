using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Fighter : MonoBehaviour
{
    public enum CharacterStates { Idle, Locked, Running, Fight, Death };
    public CharacterStates CurrentState;
    public CharacterSO characterSO;
    GameObject nearestTarget = null;
    public float MovementSpeed;
    public bool IsBoardCleared = false;
    public Transform LockedTarget;
    public float TurnSpeed = 20f;
    public GameObject SpellPrefab;
    public GameObject MeleeHitEffect;
    public Transform FirePoint;
    public Transform HitPoint;
    public float spellCooldown = 0f;
    public float MeleeHitCooldown = 0f;
    public float CurrentHealth;
    public Image HealthBar;
    private string winnerSide = "Büşra";

    private GameObject[] _enemies;
    private GameObject[] _heroes;
    private CharacterSO.CharacterType meleeType;
    private CharacterSO.CharacterType wizardType;



    void Start()
    {
        CurrentHealth = characterSO.MaxHealth;
        meleeType = CharacterSO.CharacterType.Melee;
        wizardType = CharacterSO.CharacterType.Wizard;
        CurrentState = CharacterStates.Idle;
        MovementSpeed = 1f;

    }

    void Update()
    {
        _heroes = GameObject.FindGameObjectsWithTag("Hero");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (GameManager.Instance._isStart)
        {
            switch (CurrentState)
            {
                case CharacterStates.Idle:
                    IdleState();
                    //Bir hedefe locklanmal? ve state'i locked olarak de?i?meli.
                    break;
                case CharacterStates.Locked:
                    LockedState();

                    //Birim melee ya da wizard m? diye kontrol edilmeli. Wizard ise sald?rmal?. 
                    //Birim melee ise target ile aras?ndaki farka bakacak. 2f'ten küçükse sald?racak, büyükse ko?acak.
                    break;
                case CharacterStates.Running:
                    if (nearestTarget)
                    {
                        RunningState();
                    }
                    else
                    {
                        CurrentState = CharacterStates.Idle;
                    }

                    //En yak?n mesafedeki target'a do?ru ko?acak. Aras?ndaki mesafe 2f'ten küçük olduktan sonra Sald?racak.
                    break;
                case CharacterStates.Fight:
                    FightState();
                    //En yak?n rakibe sald?racak.
                    break;
            }

            UpdateHealth();

            CheckIsBoardCleared();

            if (IsBoardCleared)
            {
                GameManager.Instance._isStart = false;
                if(winnerSide == "Enemies")
                {
                    GameManager.Instance.calculateCoin(15); //Kaybetme ödülü olarak 15 coin
                    UIManager.Instance.OnDefeat();

                }else if(winnerSide == "Heroes")
                {
                    GameManager.Instance.calculateCoin(25); //Kazanma ödülü olarak 25 coin
                    UIManager.Instance.OnVictory();
                }
            }

            if (CurrentHealth <= 0f)
            {
                Destroy(gameObject);
            }

            if (LockedTarget)
            {
                LookToTarget();
                SetStatusByType();
            }
        }
    }
    void IdleState()
    {
        if (!IsBoardCleared) //Takimlardan herhangi bir tarafin tum birimlerinin olup olmedigini kontrol eder.
        {
            FindNearestTarget(IsTargetEnemyOrHero());
            if (nearestTarget)
            {
                CurrentState = CharacterStates.Locked;
            }
        }
    }
    void LockedState()
    {
        if (nearestTarget)
        {
            LockedTarget = nearestTarget.transform;
        }
    }
    void RunningState()
    {
        float speedModifier = MovementSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, LockedTarget.position, speedModifier);
        CheckDistanceBetweenTarget();
    }
    void FightState()
    {

        if (nearestTarget)
        {
            if (characterSO.characterType == meleeType)
            {
                MeleeAttack();
            }
            else if (characterSO.characterType == wizardType)
            {
                WizardAttack();
            }
        }
        else
        {
            CurrentState = CharacterStates.Idle;
        }

    }

    void CheckIsBoardCleared()
    {
        if (_enemies.Length <= 0)
        {
            IsBoardCleared = true;
            winnerSide = "Heroes";
        }
        else if (_heroes.Length <= 0)
        {
            IsBoardCleared = true;
            winnerSide = "Enemies";
        }
    }
    void LookToTarget()
    {
        Vector3 direction = LockedTarget.position - transform.position; //Kilitlenilecek dü?man ile aradaki mesafe
        Quaternion lookRotation = Quaternion.LookRotation(direction); //Dü?man?n pozisyonuna göre karakterin dönece?i yönü belirler
        Vector3 rotation = Quaternion.Lerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * TurnSpeed).eulerAngles; //dönü? aç?s?n? belirler
        gameObject.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f); //Dönü? aç?s?na göre karakterin dönmesini sa?lar   
    }
    
    void CheckDistanceBetweenTarget()
    {
        float distanceBetweenCharacters = (Vector3.Distance(gameObject.transform.position, LockedTarget.transform.position));
        if(distanceBetweenCharacters >= 1f)
        {
            CurrentState = CharacterStates.Running;
        }
        else
        {
            CurrentState = CharacterStates.Fight;
        }
    }

    void SetStatusByType()
    {
        if (characterSO.characterType == meleeType)
        {
            CheckDistanceBetweenTarget();
        }
        else if (characterSO.characterType == wizardType)
        {
            CurrentState = CharacterStates.Fight;
        }
    }

    GameObject FindNearestTarget(GameObject[] _targets)
    {
        float distance = Mathf.Infinity;
        foreach (var target in _targets)
        {
            if (Vector3.Distance(gameObject.transform.position, target.transform.position) < distance)
            {
                nearestTarget = target;
                distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
            }
        }
        return nearestTarget;
    }
    GameObject[] IsTargetEnemyOrHero()
    {
        if (gameObject.tag == "Hero")
        {
            return _enemies;
        }
        else if (gameObject.tag == "Enemy")
        {
            return _heroes;
        }
        else
        {
            return null;
        }
    }

    void MeleeAttack()
    {
        if (nearestTarget)
        {
            if (MeleeHitCooldown <= 0f)
            {
                AttackToEnemy();
                MeleeHitCooldown = 2f / characterSO.AttackSpeed;
            }
            MeleeHitCooldown -= Time.deltaTime;
        }
        else
        {
            CurrentState = CharacterStates.Idle;
        }

    }

    void WizardAttack()
    {
        if (nearestTarget)
        {
            if (spellCooldown <= 0f)
            {
                UseSpell();
                spellCooldown = 2f / characterSO.AttackSpeed;
            }
            spellCooldown -= Time.deltaTime;
        }
        else
        {
            CurrentState = CharacterStates.Idle;
        }
    }

    public void DealDamage(float damage)
    {
        LockedTarget.GetComponent<Fighter>().CurrentHealth -= damage;
        if (LockedTarget.tag == "Enemy")//Eğer hasarı alan objenin tag i "Enemy" ise, yani hero hasar verdiyse
        {
            GameManager.Instance.coins += GameManager.Instance.meleeHitReward; //Kur Korumalı Mevduattan(Melee hero düşmana hasar verdiğinde) 1 coin kazan
            PlayerPrefs.SetInt("coins", GameManager.Instance.coins); //artırılmış sonucu hafızaya kaydet
            GameManager.Instance.coinsText.text = GameManager.Instance.coins.ToString(); // yeni coin değerini ui da yazdır
        }
        //CoinManager.balance += CoinManager.getCoin * CoinManager.coinMultipler;   
        //Burada hit başına ne kadar coin kazanılacağı hesaplanacak.
        //Belirtilen değişkenler CoinManager içerisinde tanımlanacak.

    }
    void UpdateHealth()
    {
        HealthBar.fillAmount = CurrentHealth / characterSO.MaxHealth;
    }
    void AttackToEnemy()
    {
        if (nearestTarget)
        {
            Transform targetsHitPoint = nearestTarget.GetComponent<Fighter>().HitPoint;
            GameObject effectIns = (GameObject)Instantiate(MeleeHitEffect, targetsHitPoint.position, targetsHitPoint.rotation);
            Destroy(effectIns, 1f);
            DealDamage(characterSO.AttackDamage);
        }
        else
        {
            CurrentState = CharacterStates.Idle;
        }
    }
    void UseSpell()
    {
        if (nearestTarget)
        {
            GameObject spellGO = (GameObject)Instantiate(SpellPrefab, FirePoint.position, FirePoint.rotation);
            Spells spell = spellGO.GetComponent<Spells>();
            Transform targetsHitPoint = nearestTarget.GetComponent<Fighter>().HitPoint;

            if (spell != null)
            {
                spell.Seek(targetsHitPoint, nearestTarget, characterSO.AttackDamage);
            }
        }
        else
        {
            CurrentState = CharacterStates.Idle;
        }
    }
}
/*
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
    private void OnEnable()
    {
        GameManager.onModeChange -= detectModeChange;
        GameManager.onModeChange += detectModeChange;
    }

    private void OnDestroy()
    {
        GameManager.onModeChange -= detectModeChange;
    }
    void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        if (GameManager.Instance._isStart)
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
            _agent.SetDestination(new Vector3(target.x, transform.position.y, target.z));
        }
    }

    public void detectModeChange(bool mode)
    {
        if (mode == true)
        {
            _heroes = GameObject.FindGameObjectsWithTag("Hero");
            _agent.enabled = true;
        }
        else
        {
            _agent.enabled = false;
        }
    }
}
*/
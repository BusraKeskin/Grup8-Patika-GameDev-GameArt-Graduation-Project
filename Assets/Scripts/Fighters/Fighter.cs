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

    private GameObject[] _enemies;
    private GameObject[] _heroes;
    private CharacterSO.CharacterType meleeType;
    private CharacterSO.CharacterType wizardType;



    void Start()
    {
        CurrentHealth = characterSO.CurrentHealth;
        meleeType = CharacterSO.CharacterType.Melee;
        wizardType = CharacterSO.CharacterType.Wizard;
        CurrentState = CharacterStates.Idle;
        MovementSpeed = 0.5f;

    }

    void Update()
    {
        _heroes = GameObject.FindGameObjectsWithTag("Hero");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(GameManager.Instance._isStart) 
        {
            switch (CurrentState)
            {
                case CharacterStates.Idle:
                    IdleState();
                    //Bir hedefe locklanmal? ve state'i locked olarak de?i?meli.
                    break;
                case CharacterStates.Locked:
                    StartCoroutine(LockedState());
                    
                    //Birim melee ya da wizard m? diye kontrol edilmeli. Wizard ise sald?rmal?. 
                    //Birim melee ise target ile aras?ndaki farka bakacak. 2f'ten küçükse sald?racak, büyükse ko?acak.
                    break;
                case CharacterStates.Running:
                    if(nearestTarget)
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

            if (CurrentHealth <= 0f)
            {
                Destroy(gameObject);
            }
            if (_enemies.Length <= 0)
            {
                IsBoardCleared = true;
            }
            else if(_heroes.Length <= 0)
            {
                IsBoardCleared = true;
            }
            if(IsBoardCleared)
            {
                GameManager.Instance._isStart = false;
                //Time.timeScale = 0;
            }
        }
    }
    void IdleState()
    {
        if (!IsBoardCleared) //Takimlardan herhangi bir tarafin tum birimlerinin olup olmedigini kontrol eder.
        {
            FindNearestTarget(IsTargetEnemyOrHero());
            if(nearestTarget)
            {
                CurrentState = CharacterStates.Locked;
            }
        }
    }
    IEnumerator LockedState()
    {
        while(true)
        {
            if (nearestTarget != null)
            {
                LockedTarget = nearestTarget.transform;

                Vector3 direction = LockedTarget.position - transform.position; //Kilitlenilecek dü?man ile aradaki mesafe
                Quaternion lookRotation = Quaternion.LookRotation(direction); //Dü?man?n pozisyonuna göre karakterin dönece?i yönü belirler
                Vector3 rotation = Quaternion.Lerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * TurnSpeed).eulerAngles; //dönü? aç?s?n? belirler
                gameObject.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f); //Dönü? aç?s?na göre karakterin dönmesini sa?lar          
                float distanceBetweenCharacters = (Vector3.Distance(gameObject.transform.position, LockedTarget.transform.position));

                if (characterSO.characterType == meleeType)
                {
                    if (distanceBetweenCharacters >= 1f)
                    {
                        CurrentState = CharacterStates.Running;
                    }
                    else
                    {
                        CurrentState = CharacterStates.Fight;
                    }
                }
                if (characterSO.characterType == wizardType)
                {
                    CurrentState = CharacterStates.Fight;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    void RunningState()
    {
        float speedModifier = MovementSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, LockedTarget.position, speedModifier);
    }
    void FightState()
    {
        if (nearestTarget)
        {
            if (characterSO.characterType == meleeType)
            {
                MeleeAttack();
            }
            else if(characterSO.characterType == wizardType)
            {
                WizardAttack();
            }
        }
        else
        {
            CurrentState = CharacterStates.Idle;
        }

    }

    void DeathState()
    {
        Destroy(gameObject);
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
        if (transform.parent.name == "Enemies")
        {
            return _heroes;
        }
        else if (transform.parent.name == "Heroes")
        {
            return _enemies;
        }
        else
        {
            return null;
        }
    }

    void MeleeAttack()
    {
        if(nearestTarget)
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
        if(nearestTarget)
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
        LockedTarget.GetComponent<Fighter>().characterSO.CurrentHealth = CurrentHealth;
        LockedTarget.GetComponent<Fighter>().HealthBar.fillAmount = LockedTarget.GetComponent<Fighter>().CurrentHealth / LockedTarget.GetComponent<Fighter>().characterSO.MaxHealth;
    }
    void AttackToEnemy()
    {
        if(nearestTarget)
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
        if(nearestTarget)
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
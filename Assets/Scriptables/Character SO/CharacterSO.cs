using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character Scriptable Object", order = 1)]
public class CharacterSO : ScriptableObject
{
    public int MaxHealth;
    public int AttackSpeed;
    public int AttackDamage;
    public int Test;
    public CharacterType characterType;
    public Type type;

    public enum Type
    {
        MeleeFighter_v1,
        MeleeFighter_v2,
        MeleeFighter_v3,
        Wizard_v1,
        Wizard_v2,
        Wizard_v3
    }
    public enum CharacterType
    {
        Melee,
        Wizard
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DragAndDropScriptableObject", order = 1)]

public class DragAndDropScriptable : ScriptableObject
{
    public LayerMask GridMask;
    public LayerMask AllMask;
    public LayerMask ElonMask;
}


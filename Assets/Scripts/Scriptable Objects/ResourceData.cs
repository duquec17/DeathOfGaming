using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Game/Resource")]

public class ResourceData : ScriptableObject
{
    public string resourceName;
    public Sprite icon;
    public float maxCap= 1000;
}

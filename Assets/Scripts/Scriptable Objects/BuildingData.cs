using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Data", menuName = "Building Data", order = 60)]
public class BuildingData : ScriptableObject
{
    public string buildingName = "Building Name";
    public GameObject prefab;
    public Sprite icon;

    public ResourceData resourceType;
    public float resourceIncrease;
    public float resourceCost;
}

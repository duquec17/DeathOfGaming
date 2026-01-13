using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDatabase", menuName = "Buildings/Database")]
public class BuildingDatabase : ScriptableObject
{
    public List<BuildingData> allBuildings;
}

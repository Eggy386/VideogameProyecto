using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Plant Data", menuName = "Plant Data", order = 40)]
public class PlantData : ScriptableObject
{
    public string plantName;
    public int timeToNextGrowthStage;
    public ItemData seedPackage;

    public List<Tile> growthStagesTiles = new List<Tile>();

}

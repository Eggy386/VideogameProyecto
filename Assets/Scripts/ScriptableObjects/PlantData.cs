using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Plant Data", menuName = "Plant Data", order = 40)]
public class PlantData : ScriptableObject
{
    public string plantName;
    public int sellCost;
    public ItemData seedPackage;
}

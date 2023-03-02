using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant Data", menuName = "Plant Data", order = 40)]
public class PlantData : ScriptableObject
{
    public string plantName;

    public ItemData seedPackage;

    public List<Sprite> growthStagesIcons = new List<Sprite>();
}

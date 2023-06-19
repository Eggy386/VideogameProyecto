using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
    public bool isPlantable;

    //IF IS PLANTABLE:
    
    [Header("If plantable")]
    public List<Tile> growthStagesTiles = new List<Tile>();
    public int timeToNextGrowthStage; // optionally - I'm now considering upgrading each plant everyday.
    public GameObject grownPlant;
}

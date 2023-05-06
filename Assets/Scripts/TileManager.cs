using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tilemap plantingMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile plowedTile;
    [SerializeField] private PlantData plantedTile; //for now only 1 available, later on probably will be replaced by a list Tiles

    void Start()
    {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_visible" )
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }
    }

    public bool isInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if(tile!= null)
        {
            if(tile.name == "Interactable")
                return true;
        }
        return false;
    }

    public void SetPlowed(Vector3Int position)
    {
        interactableMap.SetTile(position, plowedTile);
    }

    public bool IsPlowed(Vector3Int position)
    {
        if(interactableMap.GetTile(position) == plowedTile)
            return true;
        return false;
    }

    public void PlantSeed(Vector3Int position, Item itemToPlant)
    {
        if(itemToPlant != null && itemToPlant.data.isPlantable) // + is something already planted here?
        {
            // set tile to first stage of growth
                plantingMap.SetTile(position, itemToPlant.data.growthStagesTiles[0]);
            // forward position and item into growthManager or smth -> It will update each planted plant 
            // therefore changing it's growth stage
        }
        else
        {
            Debug.Log("Item that you are holding IS NOT plantable");
        }
    }
}

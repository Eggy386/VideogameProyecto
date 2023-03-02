using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tilemap plantingMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;
    [SerializeField] private Tile plantedTile; //for now only 1 available, later on probably will be replaced by a list Tiles

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
        interactableMap.SetTile(position, interactedTile);
    }

    public bool IsPlowed(Vector3Int position)
    {
        if(interactableMap.GetTile(position) == interactedTile)
            return true;
        return false;
    }

    public void PlantSeed(Vector3Int position)
    {
        plantingMap.SetTile(position, plantedTile);
    }
}

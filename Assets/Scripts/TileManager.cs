using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tilemap plantingMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile plowedTile;
    [SerializeField] private ItemData[] plantedTiles;
    [SerializeField] private Dictionary<Vector3Int, Item> planted = new Dictionary<Vector3Int, Item>(); // manages the position and plant that grows in that position
    [SerializeField] private Dictionary<Vector3Int, int> growthStage = new Dictionary<Vector3Int, int>();
    
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
            planted[position] = itemToPlant;
            growthStage[position] = 0;
                
                    // forward position and item into growthManager or smth -> It will update each planted plant 
            // therefore changing it's growth stage
        }
        else
        {
            Debug.Log("Item that you are holding IS NOT plantable");
        }
    }
    public void UpdateGrowthStages()
    {
        foreach(KeyValuePair<Vector3Int, Item> plant in planted)
        {
            // additionaly check if plant was watered
            if (growthStage[plant.Key] < plant.Value.data.growthStagesTiles.Count - 1)
            {
                growthStage[plant.Key] += 1;
            }
            plantingMap.SetTile(plant.Key, plant.Value.data.growthStagesTiles[growthStage[plant.Key]]);
        }
    }
    public void CollectGrownPlant(Vector3Int position)
    {
        if (growthStage[position] == planted[position].data.growthStagesTiles.Count - 1)
        {
            Debug.Log("You can collect this plant: " + planted[position].name);
            Vector2 spawnLocation = new Vector2(position.x, position.y);
            Vector2 spawnOffset = Random.insideUnitCircle.normalized * Random.Range(1.0f, 1.5f);
            GameObject plant = Instantiate(planted[position].data.grownPlant, spawnLocation + spawnOffset, Quaternion.identity);
            plant.GetComponent<Rigidbody2D>().AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
            plantingMap.SetTile(position, plowedTile);
            planted.Remove(position);
            growthStage.Remove(position);
            //droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Plant is not fully grown yet! " + planted[position].name);

        }
    }
}

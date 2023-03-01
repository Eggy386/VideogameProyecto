using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(27); // passing the num of slots we want;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Check if the tile is interactable
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            if(GameManager.instance.tileManager.isInteractable(position))
            {
                //Change the tile to plowed
                Debug.Log("Tile is interactable");
                GameManager.instance.tileManager.SetInteracted(position);
            }

        }
    }

    public void DropItem(Collectable item)
    {
        Vector2 spawnLocation = transform.position;


        Vector2 spawnOffset = Random.insideUnitCircle.normalized * Random.Range(1.0f, 1.5f);

        Collectable droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }
}

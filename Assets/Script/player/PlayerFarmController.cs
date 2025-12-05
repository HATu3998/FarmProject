using MyProject.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFarmController : MonoBehaviour
{
    public Tilemap tmGround;
    public Tilemap tmGrass;
    public Tilemap tmForest;

    public TileBase tbGround;
    public TileBase tbGrass;
    public TileBase tbForest;

    public TilemapManager tileMapManager;
    private RecyclableInventoryManager recyclableInventoryManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recyclableInventoryManager = GameObject.Find("InventoryManager").GetComponent<RecyclableInventoryManager>();

       
    }

    // Update is called once per frame
    void Update()
    {
        HandleFarmAction();

        }

    public void HandleFarmAction()
    {
        Vector3Int cellPos = tmGrass.WorldToCell(transform.position);
        if (Input.GetKeyDown(KeyCode.C))
        {
            
            TileBase crrTileBase = tmGrass.GetTile(cellPos);
            //if(crrTileBase== tbGrass)
            if(tmGrass.HasTile(cellPos))
            {
                tmGrass.SetTile(cellPos, null);
                
                if (tbGround != null)
                {
                    tmGround.SetTile(cellPos, tbGround);
                }
                tileMapManager.SetStateForTilemapDetail(cellPos.x, cellPos.y, TilemapState.Ground);
            } 
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
           
            Debug.Log("CellPos " + cellPos);
            TileBase crrTileBase = tmGrass.GetTile(cellPos);
            if(crrTileBase== null)
            {
                tmForest.SetTile(cellPos, tbForest);
                tileMapManager.SetStateForTilemapDetail(cellPos.x, cellPos.y, TilemapState.Forest);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {

            Debug.Log("CellPos " + cellPos);
            TileBase crrTileBase = tmGrass.GetTile(cellPos);
            if (crrTileBase == null)
            {
                tmGrass.SetTile(cellPos, tbGrass);
                tmForest.SetTile(cellPos, null);

                tileMapManager.SetStateForTilemapDetail(cellPos.x, cellPos.y, TilemapState.Grass);

                InvenItems itemFlower = new InvenItems();
                itemFlower.name = "hoa 1h";
                itemFlower.descrition = "hoa nay rat dep";
                Debug.Log(itemFlower.ToString());
                recyclableInventoryManager.AddInventoryItem(itemFlower);
            }
        }
    }
    
    
}

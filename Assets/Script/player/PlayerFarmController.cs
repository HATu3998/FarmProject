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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
                LogDebugPositions();
            }
            else
            {
                LogDebugPositions();
                Debug.Log("that bai - tile hi?n t?i kh?ng ph?i grass. Tile ? ?: " + cellPos + " l?: " + (crrTileBase == null ? "null" : crrTileBase.name));
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
           
            Debug.Log("CellPos " + cellPos);
            TileBase crrTileBase = tmGrass.GetTile(cellPos);
            if(crrTileBase== null)
            {
                tmForest.SetTile(cellPos, tbForest);
            }
        }
    }
    Vector3 GetTileAnchor(Tilemap t)
    {
        // ch? d?ng cho debug ¡X TileAnchor l? thu?c Tilemap component, kh?ng c? getter tr?c ti?p, n?n ??c t? Transform/Inspector n?u c?n.
        return t.transform.position; // thay th? n?u mu?n in gi? tr? transform
    }
    void LogDebugPositions()
    {
        Vector3 worldPos = transform.position;
        Debug.Log("Player worldPos: " + worldPos);
        Debug.Log("Ground pos: " + tmGround.transform.position + " Grass pos: " + tmGrass.transform.position + " Forest pos: " + tmForest.transform.position);
        Debug.Log("Ground anchor: " + GetTileAnchor(tmGround) + " Grass anchor: " + GetTileAnchor(tmGrass));
        Debug.Log("Cell by Ground: " + tmGround.WorldToCell(worldPos));
        Debug.Log("Cell by Grass: " + tmGrass.WorldToCell(worldPos));
        Debug.Log("Cell by Forest: " + tmForest.WorldToCell(worldPos));
        Debug.Log("=====================");
    }
}

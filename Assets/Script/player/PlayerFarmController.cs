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
        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector3Int cellPos = tmGround.WorldToCell(transform.position);
            TileBase crrTileBase = tmGrass.GetTile(cellPos);
            if(crrTileBase= tbGrass)
            {
                tmGrass.SetTile(cellPos, null);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {

        }
    }
}

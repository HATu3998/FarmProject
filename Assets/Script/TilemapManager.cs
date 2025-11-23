using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Tilemap tmGround;
    public Tilemap tmGrass;
    public Tilemap tmForest;
    public TileBase tbForest;
      public TileBase tbGrass;

    public TileBase[] allTiles;
    private Dictionary<string, TileBase> tileDict;
    private Map map;
    private FireBaseDatabaseManager databaseManager;
    private FirebaseUser user;
    private DatabaseReference reference;
    void Start()
    {
        map = new Map();
        databaseManager = GameObject.Find("DatabaseManager").GetComponent<FireBaseDatabaseManager>();

        user = FirebaseAuth.DefaultInstance.CurrentUser;
         //  WriteAllTileMapToFirebase();

        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        tileDict = new Dictionary<string, TileBase>();
        foreach (var tile in allTiles)
        {
            if (tile == null) continue;
            if (!tileDict.ContainsKey(tile.name))
            {
                tileDict.Add(tile.name, tile);
            }
        }
       // WriteAllTileMapToFirebase();
         loadMapForUser();
    }
    public void WriteAllTileMapToFirebase()
    {
        List<TilemapDetail> tileMaps = new List<TilemapDetail>();
        for(int x= tmGround.cellBounds.min.x; x < tmGround.cellBounds.max.x; x++)
        {
            for (int y = tmGround.cellBounds.min.y; y < tmGround.cellBounds.max.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                 
                string tileName = null;

                TileBase forestTile = tmForest.GetTile(cellPos);
                TileBase grassTile = tmGrass.GetTile(cellPos);
                TileBase groundTile = tmGround.GetTile(cellPos);

                string groundName = groundTile != null ? groundTile.name : null;
                string grassName = grassTile != null ? grassTile.name : null;
                string forestName = forestTile != null ? forestTile.name : null;
               
                
                TilemapState state = TilemapState.Ground;
                if (forestTile != null) state = TilemapState.Forest;
                else if (grassTile != null) state = TilemapState.Grass;

                TilemapDetail tmDetail = new TilemapDetail(
               x, y,
               state,
               groundName,
               grassName,
               forestName
                   );
                tileMaps.Add(tmDetail);

            }
        }
        map = new Map(tileMaps);
        Debug.Log(map.ToString());
        databaseManager.writeDatabase(user.UserId +"/Map", map.ToString());
    }

    public void loadMapForUser()
    {
        reference.Child("Users").Child(user.UserId + "/Map").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("Load map failed: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (!snapshot.Exists || snapshot.Value == null)
            {
                Debug.Log("No map found for user, creating default map from current scene");
                WriteAllTileMapToFirebase();   // map ???c g?n b?n trong
                mapToUI(map);                  // v? ra lu?n
                return;
            }
            // ?? 2. ?? C? MAP -> ??C & V?
            try
            {
                map = JsonConvert.DeserializeObject<Map>(snapshot.Value.ToString());
                Debug.Log("Load map: " + map.ToString());
                mapToUI(map);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to parse map, recreate from scene. " + e);
                WriteAllTileMapToFirebase();
                mapToUI(map);
            }




        });
       
    }
    public void TileMapDetailToTileBase(TilemapDetail tilemapDetail)
    {
        Vector3Int cellPos = new Vector3Int(tilemapDetail.x, tilemapDetail.y, 0);
        tmGround.SetTile(cellPos, null);
        tmGrass.SetTile(cellPos, null);
        tmForest.SetTile(cellPos, null);
        
        if (!string.IsNullOrEmpty(tilemapDetail.groundTileName) &&
          tileDict.TryGetValue(tilemapDetail.groundTileName, out TileBase groundTile))
        {
            tmGround.SetTile(cellPos, groundTile);
        }

        // GRASS
        if (!string.IsNullOrEmpty(tilemapDetail.grassTileName) &&
            tileDict.TryGetValue(tilemapDetail.grassTileName, out TileBase grassTile))
        {
            tmGrass.SetTile(cellPos, grassTile);
        }

        // FOREST
        if (!string.IsNullOrEmpty(tilemapDetail.forestTileName) &&
            tileDict.TryGetValue(tilemapDetail.forestTileName, out TileBase forestTile))
        {
            tmForest.SetTile(cellPos, forestTile);
        }
    }
    public void mapToUI(Map map)
    {
         for(int i=0;i< map.getLength(); i++)
        {
            TileMapDetailToTileBase(map.lstTileMapDetail[i]);
        }
    }
    public void SetStateForTilemapDetail(int x, int y,TilemapState state)
    {
        if (map == null || map.lstTileMapDetail == null || map.lstTileMapDetail.Count == 0)
        {
            Debug.LogWarning("Map empty or null, nothing to save");
            
            return;
        }
        Vector3Int cellPos = new Vector3Int(x, y, 0);
        // ??C T?T C? TILE SAU KHI PLAYER ?? THAY ??I TR?N SCENE
        TileBase groundTile = tmGround.GetTile(cellPos);
        TileBase grassTile = tmGrass.GetTile(cellPos);
        TileBase forestTile = tmForest.GetTile(cellPos);

        string groundName = groundTile != null ? groundTile.name : null;
        string grassName = grassTile != null ? grassTile.name : null;
        string forestName = forestTile != null ? forestTile.name : null;

       

        for (int i = 0; i < map.getLength(); i++)
        {
            if (map.lstTileMapDetail[i].x == x && map.lstTileMapDetail[i].y == y) {
                var cell = map.lstTileMapDetail[i];
                cell.tilemapState = state;
                cell.groundTileName = groundName;
                cell.grassTileName = grassName;
                cell.forestTileName = forestName;

                databaseManager.writeDatabase(user.UserId + "/Map", map.ToString());
                Debug.Log($"save to firebase success: ({x},{y}) state={state}, ground={groundName}, grass={grassName}, forest={forestName}");
                break;

            }
       
        }
    }
}

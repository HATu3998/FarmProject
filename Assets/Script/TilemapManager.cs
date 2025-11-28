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
    
    private FireBaseDatabaseManager databaseManager;
   
    private DatabaseReference reference;
    void Start()
    {
        
        databaseManager = GameObject.Find("DatabaseManager").GetComponent<FireBaseDatabaseManager>();
 
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

       
         loadMapForUser();
    }
    public void WriteAllTileMapToFirebase()
    {
        List<TilemapDetail> tileMaps = new List<TilemapDetail>();

        // ?? Quan tr?ng: l?y V?NG BAO CHUNG c?a 3 tilemap, kh?ng ch? Ground
        int minX = Mathf.Min(tmGround.cellBounds.xMin, tmGrass.cellBounds.xMin, tmForest.cellBounds.xMin);
        int maxX = Mathf.Max(tmGround.cellBounds.xMax, tmGrass.cellBounds.xMax, tmForest.cellBounds.xMax);
        int minY = Mathf.Min(tmGround.cellBounds.yMin, tmGrass.cellBounds.yMin, tmForest.cellBounds.yMin);
        int maxY = Mathf.Max(tmGround.cellBounds.yMax, tmGrass.cellBounds.yMax, tmForest.cellBounds.yMax);

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                TileBase forestTile = tmForest.GetTile(cellPos);
                TileBase grassTile = tmGrass.GetTile(cellPos);
                TileBase groundTile = tmGround.GetTile(cellPos);

                string groundName = groundTile != null ? groundTile.name : null;
                string grassName = grassTile != null ? grassTile.name : null;
                string forestName = forestTile != null ? forestTile.name : null;

                // state ch? ?? b?n d?ng cho logic
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


        // LoadDataManager.userInGame.MapInGame = new Map(tileMaps);

        // databaseManager.writeDatabase("Users/"+LoadDataManager.firebaseUser.UserId, LoadDataManager.userInGame.ToString());

        Debug.Log($"[WriteAllTileMapToFirebase] Generated tileMaps count = {tileMaps.Count}");

        // G?n v?o user v? l?u l?n Firebase
        if (LoadDataManager.userInGame == null)
        {
            Debug.LogError("userInGame is null when writing map!");
            return;
        }

        LoadDataManager.userInGame.MapInGame = new Map(tileMaps);

        databaseManager.writeDatabase(
            "Users/" + LoadDataManager.firebaseUser.UserId,
            LoadDataManager.userInGame.ToString()
        );
    }

    public void loadMapForUser()
    {
        //reference.Child("Users").Child(user.UserId + "/Map").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCanceled || task.IsFaulted)
        //    {
        //        Debug.LogWarning("Load map failed: " + task.Exception);
        //        return;
        //    }

        //    DataSnapshot snapshot = task.Result;

        //    if (!snapshot.Exists || snapshot.Value == null)
        //    {
        //        Debug.Log("No map found for user, creating default map from current scene");
        //        WriteAllTileMapToFirebase();   // map ???c g?n b?n trong
        //        mapToUI(map);                  // v? ra lu?n
        //        return;
        //    }
        //    // ?? 2. ?? C? MAP -> ??C & V?
        //    try
        //    {
        //        map = JsonConvert.DeserializeObject<Map>(snapshot.Value.ToString());
        //        Debug.Log("Load map: " + map.ToString());
        //        mapToUI(map);
        //    }
        //    catch (System.Exception e)
        //    {
        //        Debug.LogError("Failed to parse map, recreate from scene. " + e);
        //        WriteAllTileMapToFirebase();
        //        mapToUI(map);
        //    }

        //});

        // L?y map t? d? li?u user
        Map map = null;

        if (LoadDataManager.userInGame != null)
        {
            map = LoadDataManager.userInGame.MapInGame;
        }

        // 1. N?u map ch?a c? ho?c list r?ng -> t?o m?i t? Tilemap hi?n t?i
        if (map == null || map.lstTileMapDetail == null || map.lstTileMapDetail.Count == 0)
        {
            Debug.LogWarning("Map in user data is null or empty, generate from current scene");

            // T?o Map t? Tilemap, g?n v?o userInGame.MapInGame v? ghi l?n Firebase
            WriteAllTileMapToFirebase();

            // V? map m?i l?n Tilemap (th?c ra l?c n?y Tilemap ?? gi?ng h?t scene editor, 
            // nh?ng l?m cho ??ng flow d? li?u)
            mapToUI(LoadDataManager.userInGame.MapInGame);

            return;
        }
        if (map == null || map.lstTileMapDetail == null || map.lstTileMapDetail.Count == 0)
        {
            Debug.LogError("[loadMapForUser] After WriteAllTileMapToFirebase, map STILL empty!");
            return;
        }

        Debug.Log($"[loadMapForUser] map length = {map.getLength()}");
        // 2. N?u map ?? c? d? li?u -> v? l?i
        mapToUI(map);

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
        if (LoadDataManager.userInGame == null)
        {
            Debug.LogError("userInGame is null, cannot save map");
            return;
        }
        if (LoadDataManager.userInGame.MapInGame == null 
            || LoadDataManager.userInGame.MapInGame.lstTileMapDetail == null 
            || LoadDataManager.userInGame.MapInGame.lstTileMapDetail.Count == 0)
        {
            Debug.LogWarning("Map empty or null in SetState, regenerate from current scene");
            WriteAllTileMapToFirebase();
            if (LoadDataManager.userInGame.MapInGame == null
             || LoadDataManager.userInGame.MapInGame.lstTileMapDetail == null
             || LoadDataManager.userInGame.MapInGame.lstTileMapDetail.Count == 0)
            {
                Debug.LogError("SetState: Map still empty after regeneration, abort");
                return;
            }
        }

        Map map = LoadDataManager.userInGame.MapInGame;
        Debug.Log($"[SetState] current map length = {map.getLength()}");
        Vector3Int cellPos = new Vector3Int(x, y, 0);
        // ??C T?T C? TILE SAU KHI PLAYER ?? THAY ??I TR?N SCENE
        TileBase groundTile = tmGround.GetTile(cellPos);
        TileBase grassTile = tmGrass.GetTile(cellPos);
        TileBase forestTile = tmForest.GetTile(cellPos);

        string groundName = groundTile != null ? groundTile.name : null;
        string grassName = grassTile != null ? grassTile.name : null;
        string forestName = forestTile != null ? forestTile.name : null;

        bool found = false;

        for (int i = 0; i < map.getLength(); i++)
        {
            if (map.lstTileMapDetail[i].x == x && map.lstTileMapDetail[i].y == y)
            {
                var cell = map.lstTileMapDetail[i];
                cell.tilemapState = state;
                cell.groundTileName = groundName;
                cell.grassTileName = grassName;
                cell.forestTileName = forestName;
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogWarning($"SetStateForTilemapDetail: kh?ng t?m th?y ? ({x},{y}) trong MapInGame!");
            return;
        }
        databaseManager.writeDatabase(
       "Users/" + LoadDataManager.firebaseUser.UserId,
       LoadDataManager.userInGame.ToString()
   );
        Debug.Log($"save to firebase success: ({x},{y}) state={state}, ground={groundName}, grass={grassName}, forest={forestName}");

        
       
    }
}
 
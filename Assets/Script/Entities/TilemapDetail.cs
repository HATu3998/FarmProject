using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using Newtonsoft.Json;
public enum TilemapState
{
    Ground,
    Grass,
    Forest
}
public class TilemapDetail  
{
    public int x { get; set; }
    public int y { get; set; }
    public TilemapState tilemapState { get; set; }
    public string tileName { get; set; }
    public string groundTileName { get; set; }
    public string grassTileName { get; set; }
    public string forestTileName { get; set; }

    public TilemapDetail()
    {

    }
    public TilemapDetail(int x, int y,
                        TilemapState tilemapState,
                        string groundTileName,
                        string grassTileName,
                        string forestTileName)
    {
        this.x = x;
        this.y = y;
        this.tilemapState = tilemapState;
        this.groundTileName = groundTileName;
        this.grassTileName = grassTileName;
        this.forestTileName = forestTileName;
    }
    //public TilemapDetail(int x,int y ,TilemapState tilemapState, string tileName)
    //{
    //    this.x = x;
    //    this.y = y;
    //    this.tilemapState = tilemapState;
    //    this.tileName = tileName;
    //}
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
 
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Map { 
   
 public List<TilemapDetail> lstTileMapDetail { get; set; }

    public Map()
    {
        lstTileMapDetail = new List<TilemapDetail>();
    }
    public Map(List<TilemapDetail> lstTileMapDetail)
    {
        this.lstTileMapDetail = lstTileMapDetail ?? new List<TilemapDetail>();
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
    public int getLength()
    {
        return lstTileMapDetail.Count;
    }
}

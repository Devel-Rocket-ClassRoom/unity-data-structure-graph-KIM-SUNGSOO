using TMPro;
using UnityEngine;

public enum Sides
{
    Bottom,
    Right,
    Left,
    Top,
}
public class Tile 
{
    public int id;
    public Tile[] adjacents = new Tile[4];

    public int autoTiledid;

    public bool isVisited = false; //안개 오픈 기준

    public void UpdateAutoTileId()
    {
        autoTiledid = 0;
        for(int i =0; i<adjacents.Length; i++)
        {
            if (adjacents[i] != null)
            {
                autoTiledid |= 1 << adjacents.Length - 1 - i;
            }
        }
    }

}

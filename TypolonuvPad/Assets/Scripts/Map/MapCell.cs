using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCell
{

    public Vector3Int Coords;


    public MapCell(int x, int y) {

        Coords = new Vector3Int(x, y, 0);
    }


    // Change temporarly sprite (over)
    public void ChangeSprite(Tilemap over, Tile model)
    {

        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = model.sprite;
        over.SetTile(Coords, tile);
    }

    // Remove temp sprite
    public void RemoveSprite(Tilemap over)
    {

        if (over.GetTile(Coords) == null)
            return;

        over.SetTile(Coords, null);
    }

    // Has tile set
    public bool HasTile(Tilemap over) {

        //return over.GetTile(Coords) != null;
        var cell = over.GetTile(Coords);
        return cell != null;
    }

    // Gets tile name in lowercase
    public string GetName(Tilemap tm, Vector3Int coords) {

        var tile = tm.GetTile(coords);
        return tile.name.ToLower();
    }
}

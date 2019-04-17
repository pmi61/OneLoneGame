using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapGenerator
{
    public static Tilemap GenerateTilemap(Tile[] tiles, int width, int height)
    {
        Tilemap tilemap = new Tilemap();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[y * width + x]);
            }
        }


        return tilemap;
    }
}


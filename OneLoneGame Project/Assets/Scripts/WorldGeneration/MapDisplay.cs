using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public TilemapRenderer tilemapRenderer;

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 0, texture.height);
    }

    //public void DrawTilemap(Tilemap tilemap)
    //{
    //    tilemapRenderer.GetComponent<Tilemap>().SetTile(new Vector3Int(0, 0, 0), tilemap.GetTile(new Vector3Int(0, 0, 0)));
    //}

    public void DrawTilemap(Tile[] tiles, int width, int height)
    {
        Tilemap tilemap = tilemapRenderer.GetComponent<Tilemap>();
        tilemap.ClearAllTiles();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tilemap.SetTile(new Vector3Int(y, width - x - 1, 0), tiles[y * width + x]);
            }
        }
        tilemapRenderer.transform.localScale = new Vector3((float)width, (float)height, 0);
    }
}

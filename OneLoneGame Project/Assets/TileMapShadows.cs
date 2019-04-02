using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[ExecuteInEditMode]
public class TileMapShadows : MonoBehaviour
{
    public TilemapRenderer s;
    // Start is called before the first frame update
    void Start()
    {
        s.receiveShadows = true;
        s.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

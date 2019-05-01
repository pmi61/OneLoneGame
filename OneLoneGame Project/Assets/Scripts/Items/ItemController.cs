using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemController : NetworkBehaviour
{
    [SerializeField]
    public ItemData itemData;

    /// <summary>
    /// Маска слоя предметов
    /// </summary>
    public static LayerMask layerMask;
        
    /// <summary>
    /// ID для мультиплеера
    /// </summary>
    public int ID = 0;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Items");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    public ItemData itemData;

    /// <summary>
    /// Маска слоя предметов
    /// </summary>
    public static LayerMask layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Items");
    }
}

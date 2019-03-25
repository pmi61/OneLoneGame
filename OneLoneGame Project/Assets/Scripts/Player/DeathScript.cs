using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public List<GameObject> Loot;
   /// <summary>
   /// функция, которая должна быть вызвана перед destroy()
   /// </summary>
   /// <param name="position" - место спавна вещей></param>
    public void OnDeath(Vector2 position)
    {
        foreach(GameObject item in Loot)
        {
           GameObject i = Instantiate(item);
            i.transform.position = position;
        }
    }
}

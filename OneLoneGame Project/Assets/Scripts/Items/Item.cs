using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public string name = "Some name";
    public string description = "Some item.";
    public int maxInInvintoryCell = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear() == true)
        {
            attemptAddToPlayer();
        }
        else {
        }
    }

    private bool attemptAddToPlayer()
    {

        return true;
    }

    private bool isPlayerNear()
    {
        Debug.Log("Check distance between Item + \"" + this.name + "\" and player");



        return true;
    }
}

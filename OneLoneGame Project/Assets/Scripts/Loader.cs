using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager; // Префаб GameManager, который инстанциируется

    void Awake()
    {
        Debug.Log("Loader Awake");

        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
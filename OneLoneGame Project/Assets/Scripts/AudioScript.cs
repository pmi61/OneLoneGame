using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    private void Awake()
    {
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClip : MonoBehaviour
{
    private void Awake()
    {
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}

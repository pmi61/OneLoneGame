using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
       GetComponent<Light>().enabled = !GameManager.instance.Sun.enabled;
    }
}

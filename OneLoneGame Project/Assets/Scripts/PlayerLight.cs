using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GetComponent<Light>().intensity = 1 - GameManager.instance.TimeControl.Sun.intensity;
    }
}

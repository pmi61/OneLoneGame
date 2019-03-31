using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighintScript : MonoBehaviour
{
    public float speed;
    public float morningMultiplier;
    public float daytimeMultiplier;
    public float eveningMultiplier;
    public float nightMultiplier;
  // private Vector3 Axis = new Vector3(1, 0, 0); //ось x
   public  Light _light;
   public enum state { morning, daytime, evening, night };
    public state time;

    float n_intensity;
    void Start()
    {
        _light = GetComponent<Light>();

        n_intensity = _light.intensity;
    }
    void Update()
    {
       
        changeIntensity();
        if (GameManager.instance.TimeControl.timeInHours > 23.7 || GameManager.instance.TimeControl.timeInHours < 0.3)
            setTime(state.night, 1, 0);
        if (GameManager.instance.TimeControl.timeInHours > 5.7 && GameManager.instance.TimeControl.timeInHours < 6.3)
            setTime(state.morning, 1, 0.4f);
        if (GameManager.instance.TimeControl.timeInHours > 11.7 && GameManager.instance.TimeControl.timeInHours < 12.3)
            setTime(state.daytime, 1, 1);
        if (GameManager.instance.TimeControl.timeInHours > 16.7 && GameManager.instance.TimeControl.timeInHours < 17.3)
            setTime(state.evening, 1, 0.4f);
       
    }
    void setTime(state To, float durationInHours, float intensity)
    {
        time = To;
        n_intensity = intensity;
    }

    void changeIntensity()
    {
        _light.intensity = Mathf.MoveTowards(_light.intensity, n_intensity, 0.5f * Time.deltaTime);
    }
   
}

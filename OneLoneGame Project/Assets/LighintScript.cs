using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighintScript : MonoBehaviour
{
    public float speed;
    private Vector3 Axis = new Vector3(1, 0, 0); //ось x
   public  Light _light;

    void Start()
    {
        //_light = GetComponent<Light>();
    }
    void Update()
    {
        float angle;
        transform.rotation *= Quaternion.AngleAxis(speed * Time.deltaTime, Axis);  //получаем кватернион и совершаем поворот
        if (transform.rotation.eulerAngles.y > 179)
            angle = 180 - transform.rotation.eulerAngles.x;
        else
            angle = transform.rotation.eulerAngles.x;
        if (angle > 0 && angle  < 90) // 0 и 90 смотрел в редакторе юнити, это те цифры, когда день
            _light.enabled = true;
        else //иначе ночь 
            _light.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public Light sun;
    public Light Sun { get { return sun; } }

    public float timeInHours;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
       timeInHours = 12.0f;
        sun.intensity = 1;
        sun.GetComponent<LighintScript>().time = LighintScript.state.daytime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsInMenu || !GameManager.instance.isGameRunning)
            return;
        string t = ((int)timeInHours).ToString() + ":";
        float d = timeInHours - (int)timeInHours;
        d = Mathf.Round(Mathf.Round(d * 100f) * 0.6f);
        if (d < 10)
            t += "0" + d;
        else
            t += + d;
        timeText.SetText(t);
        timeInHours += speed * Time.deltaTime;
        if (timeInHours >= 24)
            timeInHours = 0;
    }
}

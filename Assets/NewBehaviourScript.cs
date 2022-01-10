using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Slider rotslider;
    public Slider scaleSlider;
    public float scaleMinVal;
    public float scaleMaxVal;

    public float rotMinVal;
    public float rotMaxVal;
    // Start is called before the first frame update
    void Start()
    {
        scaleSlider = GameObject.Find("ScaleSlide").GetComponent<Slider>();
        scaleSlider.minValue = scaleMinVal;
        scaleSlider.maxValue = scaleMaxVal;
        scaleSlider.onValueChanged.AddListener(scalesliderUpdate);

        rotslider = GameObject.Find("RotateSlide").GetComponent<Slider>();
        rotslider.minValue = rotMinVal;
        rotslider.maxValue = rotMaxVal;
        rotslider.onValueChanged.AddListener(rotsliderUpdate);
    }

    void rotsliderUpdate(float value)
    {
        transform.localEulerAngles=new Vector3(transform.rotation.x, value, transform.rotation.y);
    }

    void scalesliderUpdate(float value)
    {
        transform.localScale = new Vector3(value, value, value);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

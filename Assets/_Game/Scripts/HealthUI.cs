using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthSlider3d;
    public Slider healthSlider2d;


    public void Start3DSlider(float maxValue)
    {
        healthSlider3d.maxValue = maxValue;
        healthSlider3d.value = maxValue;
    }

    public void Update3DSlider(float value)
    {
        healthSlider3d.value = value;
    }


    public void Update2DSlider(float maxValue, float value)
    {
        if (gameObject.CompareTag("Player"))
        {
            healthSlider2d.maxValue = maxValue;
            healthSlider2d.value = value;
        }
    }
}

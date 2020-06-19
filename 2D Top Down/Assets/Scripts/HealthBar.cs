using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetMaxHealth(int health)
    {
        //set max value of slider
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(int health)
    {
        //set the slider to current health
        slider.value = health;
    }
}

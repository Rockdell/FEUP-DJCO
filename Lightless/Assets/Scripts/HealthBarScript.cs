using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class HealthBarScript : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI text;

    void Awake() {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(float maxHp) {
        slider.maxValue = maxHp;
        slider.value = maxHp;
        SetText(maxHp);
    }

    public void SetHealth(float hp) {
        slider.value = hp;
        SetText(hp);
    }

    private void SetText(float number) {
        text.text = ((int)number).ToString();
    }

}

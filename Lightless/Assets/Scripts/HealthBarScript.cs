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

    public void SetMaxHealth(int maxHp) {
        slider.maxValue = maxHp;
        slider.value = maxHp;
        text.text = maxHp.ToString();
    }

    public void SetHealth(int hp) {
        slider.value = hp;
        text.text = hp.ToString();
    }

}

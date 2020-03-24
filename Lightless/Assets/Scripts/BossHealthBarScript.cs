using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarScript : MonoBehaviour {

    public float hurtSpeed;
    public TextMeshProUGUI text;
    private Image fill;
    private Image fillBackground;
    private float maxHP;

    void Awake() {
        fillBackground = transform.GetChild(0).GetComponent<Image>();
        fill = transform.GetChild(1).GetComponent<Image>();
    }

    public void SetMaxHealth(float maxHp) {
        fill.fillAmount = 1;
        fillBackground.fillAmount = 1;
        maxHP = maxHp;
        SetText(maxHp);
    }

    public void SetHealth(float hp) {
        fill.fillAmount = hp / maxHP;
        SetText(hp);
    }

    private void SetText(float number) {
        text.text = ((int)number).ToString();
    }

    void Update() {
        if (fill.fillAmount < fillBackground.fillAmount)
            fillBackground.fillAmount -= hurtSpeed;
        else
            fillBackground.fillAmount = fill.fillAmount;
    }

}

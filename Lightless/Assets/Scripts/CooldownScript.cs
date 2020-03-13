using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownScript : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float opacity;
    public RectTransform cooldownCircle;
    public RectTransform cooldownIcon;

    private float maxCooldown;
    private Image imageCircle;
    private Image imageIcon;
    private Color onCooldownColor;
    private Color offCooldownColor;

    void Awake() {
        imageCircle = cooldownCircle.GetComponent<Image>();
        imageIcon = cooldownIcon.GetComponent<Image>();
        onCooldownColor = new Color(1, 1, 1, opacity);
        offCooldownColor = new Color(1, 1, 1, 1);
    }

    public void SetMaxCooldown(float cooldown) {
        maxCooldown = cooldown;
        imageCircle.fillAmount = 1;
    }

    public void SetCooldown(float cooldown) {
        imageCircle.fillAmount = cooldown / maxCooldown;

        if (cooldown < maxCooldown)
            imageIcon.color = onCooldownColor;
        else
            imageIcon.color = offCooldownColor;
    }
}

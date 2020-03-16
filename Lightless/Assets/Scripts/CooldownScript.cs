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
    private bool isTimer;
    private Image imageCircle;
    private Image imageIcon;
    private Color opaque;
    private Color transparent;

    void Awake() {
        imageCircle = cooldownCircle.GetComponent<Image>();
        imageIcon = cooldownIcon.GetComponent<Image>();
        transparent = new Color(1, 1, 1, opacity);
        opaque = new Color(1, 1, 1, 1);
    }

    public void SetMaxCooldown(float cooldown, bool behaveAsTimer = false) {
        maxCooldown = cooldown;
        isTimer = behaveAsTimer;
        imageCircle.fillAmount = 1;

        if (isTimer)
            imageIcon.color = opaque;
    }

    public void SetCooldown(float cooldown) {
        imageCircle.fillAmount = cooldown / maxCooldown;

        if (isTimer) {
            if (cooldown > 0)
                imageIcon.color = opaque;
            else
                imageIcon.color = transparent;
        }
        else {
            if (cooldown < maxCooldown)
                imageIcon.color = transparent;
            else
                imageIcon.color = opaque;
        }
    }
}

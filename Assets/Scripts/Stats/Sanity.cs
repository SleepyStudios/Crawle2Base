using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sanity : MonoBehaviour {
    Image bar;
    Light globalLight;
    int sanity = 100;
    float tmrSanity;

    void Start() {
        bar = GameObject.Find("Sanity Bar").GetComponent<Image>();
        globalLight = GameObject.Find("Global Light").GetComponent<Light>();
    }

    void Update() {
        if (bar == null) return;

        tmrSanity += Time.deltaTime;
        if (tmrSanity >= 3.0f && isNight()) {
            sanity -= 1;
            tmrSanity = 0;
        }

        bar.fillAmount = Mathf.Lerp(bar.fillAmount, sanity / 100f, 0.1f);
    }

    bool isNight() {
        return globalLight.color.r <= 0.4f;
    }
}

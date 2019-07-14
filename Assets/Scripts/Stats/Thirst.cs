using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thirst : MonoBehaviour {
    Image bar;
    int thirst = 100;
    float tmrThirst;

    void Start() {
        bar = GameObject.Find("Thirst Bar").GetComponent<Image>();
    }

    void Update() {
        if (bar == null) return;

        tmrThirst += Time.deltaTime;
        if (tmrThirst >= 2.0f) {
            thirst -= 2;
            tmrThirst = 0;
        }

        bar.fillAmount = Mathf.Lerp(bar.fillAmount, thirst / 100f, 0.1f);
    }
}

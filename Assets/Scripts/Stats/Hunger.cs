using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour {
    Image bar;
    int hunger = 100;
    float tmrHunger;

	void Start () {
        bar = GameObject.Find("Hunger Bar").GetComponent<Image>();
    }

    void Update () {
        if (bar == null) return;

        tmrHunger += Time.deltaTime;
        if(tmrHunger >= 3.0f) {
            hunger -= 1;
            tmrHunger = 0;
        }

        bar.fillAmount = Mathf.Lerp(bar.fillAmount, hunger / 100f, 0.1f);
    }
}

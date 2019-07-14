using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour {
    public bool running;
    public float restTime = 2.0f;
    Image bar;
    int energy = 100;
    float tmrRest, tmrEnergy; 
    bool resting;

    void Start() {
        bar = GameObject.Find("Energy Bar").GetComponent<Image>();
    }

    void Update() {
        if (bar == null) return;

        int xUnitSpeed = (int)Input.GetAxis("Horizontal");
        int yUnitSpeed = (int)Input.GetAxis("Vertical");
        running = Input.GetButton("Run") && (xUnitSpeed != 0 || yUnitSpeed != 0) && energy > 0;

        // Wait until they've ran out of energy to rest
        if ((energy == 0 || Input.GetButtonUp("Run")) && !resting) resting = true;
        if (resting && !Input.GetButton("Run")) {
            tmrRest += Time.deltaTime;
            if (tmrRest >= restTime) {
                resting = false;
                tmrRest = 0;
            }
        }

        // Drain or replenish energy
        tmrEnergy += Time.deltaTime;
        if (running) {
            if (tmrEnergy >= 0.05f && energy > 0) {
                energy -= 1;
                tmrEnergy = 0;
            }
        } else if (!resting) {
            if (tmrEnergy >= 1.5f && energy < 100) {
                energy += 2;
                if (energy > 100) energy = 100;
                tmrEnergy = 0;
            }
        }

        bar.fillAmount = Mathf.Lerp(bar.fillAmount, energy / 100f, 0.1f);
    }
}

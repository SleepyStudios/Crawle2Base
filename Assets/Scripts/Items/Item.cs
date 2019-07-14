using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public string name;
    public string icon;
    public Sprite iconSprite;

	public void Init() {
        Sprite sprite = Resources.Load<Sprite>("Items/" + icon);
        iconSprite = sprite;
    }
}

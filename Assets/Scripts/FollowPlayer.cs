using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public Transform player;
    private float leftBound, rightBound, bottomBound, topBound;
    private SpriteRenderer spriteBounds;

    void Start () {
        float vertExtent = GetComponent<Camera>().orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        SpriteRenderer background = GameObject.Find("Grass").GetComponentInChildren<SpriteRenderer>();
        leftBound = background.bounds.min.x + horzExtent;
        rightBound = background.bounds.max.x - horzExtent;
        bottomBound = background.bounds.min.y + vertExtent;
        topBound = background.bounds.max.y - vertExtent;
    }

    void LateUpdate () {
        if (player == null) return;
        
        var pos = new Vector3(player.position.x, player.position.y, -1);
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
        pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
        transform.position = pos;
    }
}

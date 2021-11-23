using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartText : MonoBehaviour
{
    //private float verticalDistance = 1f;
    //private float offset = 5f;

    // Update is called once per frame
    private void Update()
    {
        // Text Vertical Movement
        //float y = Mathf.PingPong(Time.time / 2f, verticalDistance) + offset;
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, 0);

        // Text Scale
        float scale = Mathf.PingPong(Time.time / 4f, 0.2f) + 1;
        gameObject.transform.localScale = new Vector3(scale, scale, gameObject.transform.localScale.z);

        // Destroy this text if space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private float DESTROY_TIME = 2f;
    private Quaternion rot;
    private TextAnchor[] textAnchorOptions = { TextAnchor.UpperLeft, TextAnchor.UpperCenter, TextAnchor.UpperRight, TextAnchor.MiddleLeft, TextAnchor.MiddleCenter, TextAnchor.MiddleRight };

    void Start()
    {
        //Destroy(gameObject, DESTROY_TIME);
        rot = transform.rotation;
        TextMesh text = GetComponent<TextMesh>();
        text.anchor = textAnchorOptions[Random.Range(0, textAnchorOptions.Length-1)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rot;
    }
}

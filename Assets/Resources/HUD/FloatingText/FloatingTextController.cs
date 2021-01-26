using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private float DESTROY_TIME = 2f;
    private Quaternion rot;
    private TextAnchor[] textAnchorOptions = { TextAnchor.UpperLeft, TextAnchor.UpperCenter, TextAnchor.UpperRight, TextAnchor.MiddleLeft, TextAnchor.MiddleCenter, TextAnchor.MiddleRight };
    //private Vector3 offset = new Vector3(0f,30f,0f);
    //private Vector3 randomizePosition = new Vector3(50f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, DESTROY_TIME);
        rot = transform.rotation;
        TextMesh text = GetComponent<TextMesh>();
        text.anchor = textAnchorOptions[Random.Range(0, textAnchorOptions.Length-1)];
        Debug.Log(text.anchor);
        //transform.localPosition += offset;
        //transform.localPosition += new Vector3(Random.Range(-randomizePosition.x, randomizePosition.x),
        //    Random.Range(-randomizePosition.y, randomizePosition.y),
        //    Random.Range(-randomizePosition.z, randomizePosition.z));
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rot;
    }
}

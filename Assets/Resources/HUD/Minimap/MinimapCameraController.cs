using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Camera camera;
    public Shader replacementShader;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Awake() {
        camera.SetReplacementShader(replacementShader,null);
    }
}

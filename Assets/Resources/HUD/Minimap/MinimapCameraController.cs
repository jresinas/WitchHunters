using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    private float MIN_X = -15f;
    private float MAX_X = 40f;
    private float MIN_Z = 80f;
    private float MAX_Z = 100f;

    public Camera cam;
    public Shader replacementShader;
    public GameObject player;
    public int size = 0;

    void Awake() {
        cam.SetReplacementShader(replacementShader,null);
        player = GameManager.instance.player;
    }

    // Start is called before the first frame update
    void Start() {
        //player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update() {
        // In small minimap, camera follow player. In large minimap, camera is fixed
        if (size == 0) {
            float xPos = Mathf.Clamp(player.transform.position.x, MIN_X, MAX_X);
            float zPos = Mathf.Clamp(player.transform.position.z, MIN_Z, MAX_Z);
            transform.position = new Vector3(xPos, transform.position.y, zPos);
        } else {
            transform.position = new Vector3(10, transform.position.y, 90);
        }
    }

}

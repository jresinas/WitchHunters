using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;

    RaycastHit hit;
    List<MeshRenderer> transparentBuildings = new List<MeshRenderer>();


    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0f, 15f, -25f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;

        
        RemoveTransparency();
        SetTransparency();
        
    }

    private void RemoveTransparency() {
        for (int i = 0; i<transparentBuildings.Count; i++){
            MeshRenderer build = transparentBuildings[i];
            build.material.color = new Color(1f, 1f, 1f, 1f);
            transparentBuildings.Remove(build);
        }
    }

    private void SetTransparency() {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, (player.transform.position - transform.position).magnitude)) {
            if (hit.collider.name != "Player") {
                MeshRenderer mesh = hit.collider.GetComponent<MeshRenderer>();
                mesh.material.color = new Color(1f, 1f, 1f, 0.1f);
                transparentBuildings.Add(mesh);
            }
        }
    }
}

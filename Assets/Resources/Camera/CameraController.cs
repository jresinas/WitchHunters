using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;

    RaycastHit hit;
    List<Renderer> transparentBuildings = new List<Renderer>();

    public Material transparent;
    public Material opaque;


    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0f, 15f, -25f);

        //opaque = Resources.Load("Assets/Resources/Environment/Village/Houses/fbx/House_1_unity/material_opaque.mat", typeof(Material)) as Material;
        //transparent = Resources.Load("Assets/Resources/Environment/Village/Houses/fbx/House_1_unity/material_transparent.mat", typeof(Material)) as Material;
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
            Renderer build = transparentBuildings[i];
            build.material = opaque;
            //build.material.color = new Color(1f, 1f, 1f, 1f);
            //build.enabled = true;
            transparentBuildings.Remove(build);
        }
    }

    private void SetTransparency() {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, (player.transform.position - transform.position).magnitude)) {
            if (hit.collider.name != "Player") {
                SetAllChildMeshTransparent(hit.collider);
                //MeshRenderer mesh = hit.collider.GetComponent<MeshRenderer>();
                //mesh.material.color = new Color(1f, 1f, 1f, 0.1f);
                //transparentBuildings.Add(mesh);
            }
        }
    }

    private void SetAllChildMeshTransparent(Collider obj) {
        Renderer[] childs = obj.GetComponentsInChildren<Renderer>();
        Debug.Log(obj + " " +childs.Length);
        foreach (Renderer mesh in childs) {
            mesh.material = transparent;
            //mesh.material.color = new Color(1f, 1f, 1f, 0.01f);
            //mesh.enabled = false;
            transparentBuildings.Add(mesh);
        }
    }


}

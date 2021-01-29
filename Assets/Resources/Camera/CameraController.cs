using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;

    //RaycastHit hit;
    RaycastHit[] hits;
    List<Renderer> transparentBuildings = new List<Renderer>();

    public Material houseTransparent;
    public Material houseOpaque;
    public Material churchTransparent;
    public Material churchOpaque;


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
            if (build != null) {
                switch (build.transform.parent.tag) {
                    case ("House"):
                        build.material = houseOpaque;
                        break;
                    case ("Church"):
                        build.material = churchOpaque;
                        break;
                }

                switch (build.transform.tag) {
                    case ("House"):
                        build.material = houseOpaque;
                        break;
                    case ("Church"):
                        build.material = churchOpaque;
                        break;
                }
            }
            transparentBuildings.Remove(build);
        }
    }

    private void SetTransparency() {
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), (player.transform.position - transform.position).magnitude);
        foreach (RaycastHit hit in hits) { 
            if (hit.collider.name != "Player") {
                SetAllChildMeshTransparent(hit.collider);
                SetMeshTransparent(hit.collider);
            }
        }
    }

    private void SetAllChildMeshTransparent(Collider obj) {
        Renderer[] childs = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer mesh in childs) {
            switch (mesh.transform.parent.tag) {
                case ("House"):
                    mesh.material = houseTransparent;
                    break;
                case ("Church"):
                    mesh.material = churchTransparent;
                    break;
            }
            transparentBuildings.Add(mesh);
        }
    }

    private void SetMeshTransparent(Collider obj) {
        Renderer mesh = obj.GetComponent<Renderer>();
        if (mesh != null) {
            switch (mesh.transform.tag) {
                case ("House"):
                    mesh.material = houseTransparent;
                    break;
                case ("Church"):
                    mesh.material = churchTransparent;
                    break;
            }
            transparentBuildings.Add(mesh);
        }
    }

}

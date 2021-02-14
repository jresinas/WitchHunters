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
            Renderer build = transparentBuildings[i];
            if (build != null) {
                BuildingController bc = build.GetComponentInParent<BuildingController>();
                if (bc != null) {
                    build.material = bc.opaqueMaterial;
                }
            }
            transparentBuildings.Remove(build);
        }
    }

    private void SetTransparency() {
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), (player.transform.position - transform.position).magnitude);
        foreach (RaycastHit hit in hits) { 
            if (hit.collider.name != "Player") {
                SetMeshTransparent(hit.collider);
            }
        }
    }

    private void SetMeshTransparent(Collider obj) {
        Renderer[] childs = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer mesh in childs) {
            if (mesh != null) {
                BuildingController bc = mesh.GetComponentInParent<BuildingController>();
                if(bc != null) {
                    mesh.material = bc.transparentMaterial;
                    transparentBuildings.Add(mesh);
                }
            }
        }
    }
}

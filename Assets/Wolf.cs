using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] GameObject ragdoll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DamageReceived(float amount, Vector3? _origin = null) {
        Vector3 origin = _origin != null ? (Vector3)_origin : transform.position;
        Vector3 direction = (transform.position - origin).normalized;
        //GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        //text.GetComponent<TextMesh>().text = amount.ToString();

        Dead(direction, amount * 10f);
    }

    // Enemy die
    private void Dead(Vector3 direction, float force) {
        GameObject rd = Instantiate(ragdoll, transform.position, transform.rotation);
        RagDollController rdc = rd.GetComponent<RagDollController>();
        rdc.Push(direction, force);
        Destroy(gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleMode : MonoBehaviour
{
    private float timeOut = 1;
    private float timeElapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbody���擾
        Vector3 force = new Vector3(Random.Range(-1, 1),    // X��
                                    Random.Range(-1.0f, 0.0f), // Y��
                                    Random.Range(-1, -2));   // Z��
        rb.AddForce(force, ForceMode.Impulse);          // �͂�������
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += 1 * Time.deltaTime;

        transform.Rotate(new Vector3(5, 0, 5));

        if (timeElapsed >= timeOut)
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttobi : MonoBehaviour
{
    private float timeOut = 5;
    private float timeElapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbody���擾
        Vector3 force = new Vector3(Random.Range(-5, 5),    // X��
                                    Random.Range(5.0f, 6.0f), // Y��
                                    Random.Range(-1,-5));   // Z��
        rb.AddForce(force, ForceMode.Impulse);          // �͂�������
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += 1 * Time.deltaTime;

        if (timeElapsed >= timeOut)
        {
            //Instantiate(m_objMist, this.transform.position, Quaternion.identity);
            //������
            //timeElapsed = 0.0f;
            Destroy(this.gameObject);
        }
    }
}

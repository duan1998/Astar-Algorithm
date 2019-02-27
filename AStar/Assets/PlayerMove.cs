using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test18
{
    public class PlayerMove : MonoBehaviour
    {

        public float moveSpeed;
        public Rigidbody m_rigidbody;
        // Update is called once per frame

        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

        }

        private void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            m_rigidbody.velocity = new Vector3(h, 0, v) * moveSpeed;
        }

    }

}

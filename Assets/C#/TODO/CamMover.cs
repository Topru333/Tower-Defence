using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD {
    public class CamMover : MonoBehaviour {
        public float speed = 100f;
        private Vector3 p1, p2, p3, p4;

        // Use this for initialization
        void Start () {
            TowerGridSystem.Instance.GetBBox(out p1, out p2, out p3, out p4);
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            transform.position = new Vector3((p2.x - p1.x) / 2, p2.y + 25f, p2.z);

        }

        // Update is called once per frame
        void Update () {
            transform.LookAt(new Vector3(((p2.x - p1.x) / 2), p2.z, ((p4.z - p1.z) / 2)));
            
        }

        public void Up () {
            if(transform.position.z < p3.z) {
                transform.Translate(Vector3.forward * Time.deltaTime * speed,Space.World);
            }
        }

        public void Down () {   
            if (transform.position.z > p1.z) {
                transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);
            }
        }
    }
}

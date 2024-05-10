using System;
using UnityEngine;

namespace com.ethnicthv
{
    public class CollectableBehaviour: MonoBehaviour
    {
        private Vector3 _position;

        private void Start()
        {
            _position = transform.position;
        }

        private void FixedUpdate()
        {
            //Move the collectable up and down
            transform.position = new Vector3(_position.x, _position.y + Mathf.Sin(Time.time) * 0.1f, _position.z);
        }
    }
}
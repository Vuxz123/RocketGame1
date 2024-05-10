using System;
using com.ethnicthv.Movement;
using com.ethnicthv.Outer.Behaviour.Movement;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.ethnicthv
{
    public class PlayerController: MonoBehaviour
    {
        Vector3 _direction;
        private int _d = 0;
        Vector3 _currentPosition;
        
        RotationMovementBehaviour _rotationMovementBehaviour;
        PositionMovementBehaviour _positionMovementBehaviour;
        
        public MapBehaviour mapBehaviour;
        
        
        
        private void Start()
        {
            _currentPosition = transform.position;
            _direction = Vector3.forward;

            _rotationMovementBehaviour = gameObject.AddComponent<RotationMovementBehaviour>();
            _positionMovementBehaviour = gameObject.AddComponent<PositionMovementBehaviour>();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) && mapBehaviour.CheckPlayerMovable(_currentPosition, _d))
            {
                _currentPosition += _direction;
                _positionMovementBehaviour.MoveTo(_currentPosition);
                mapBehaviour.UpdatePlayerPosition(_currentPosition);
            }
            else if (Input.GetKeyDown(KeyCode.S)&& mapBehaviour.CheckPlayerMovable(_currentPosition, _d + 180))
            {
                _currentPosition -= _direction;
                _positionMovementBehaviour.MoveTo(_currentPosition);
                mapBehaviour.UpdatePlayerPosition(_currentPosition);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _d -= 90;
                _direction = Quaternion.Euler(0, -90, 0) * _direction;
                _rotationMovementBehaviour.MoveTo(Quaternion.Euler(0, _d, 0));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _d += 90;
                _direction = Quaternion.Euler(0, 90, 0) * _direction;
                _rotationMovementBehaviour.MoveTo(Quaternion.Euler(0, _d, 0));
            }
        }
    }
}
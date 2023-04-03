using UnityEngine;

public class PlayerHorizontalMovement : MonoBehaviour
{
    PlayerController _controller;
    PlayerData _data;
    Rigidbody2D _rigidBody;
    PlayerInput _playerInput;

    Vector2 _newVelocity;
    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _data = GetComponent<PlayerData>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }

    public void HorizontalMove()
    {
        if (_controller.CollisionInfo.left == false && false == _controller.CollisionInfo.right)
        {
            if (_playerInput.IsInputXY)
            {
                _newVelocity = new Vector2(_playerInput.X * _data.MoveSpeed, _rigidBody.velocity.y);
                _rigidBody.velocity = _newVelocity;
            }
        }
    }
}

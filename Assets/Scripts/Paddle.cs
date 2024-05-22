using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public KeyCode KeyCodeUp = KeyCode.W;
    public KeyCode KeyCodeDown = KeyCode.S;

    [Header("Movement Tuning")]
    [Tooltip("The multiplier for how much of the speed is transfered to the ball on collision")]
    [SerializeField] private float _speedTransfer = 0.9f;
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _decceleration = 3f;
    [SerializeField] private float _noMoveThresh = 0.2f;

    private Rigidbody2D _rigidbody2D;
    private bool _upPressed = false;
    private bool _downPressed = false;
    private bool _onWall = false;
    private float _currentVelocity = 0f;
    private Vector2 _velocity = Vector2.zero;

    private bool _touchingWall = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
        MovePaddle();
    }

    // Update velocity with a little acceleration / decceleration
    private void UpdateVelocity()
    {
        if (Input.GetKeyDown(KeyCodeUp)) _upPressed = true;
        if (Input.GetKeyUp(KeyCodeUp)) _upPressed = false;
        if (Input.GetKeyDown(KeyCodeDown)) _downPressed = true;
        if (Input.GetKeyUp(KeyCodeDown)) _downPressed = false;

        if (_upPressed && _downPressed || !_upPressed && !_downPressed)
        {
            //_currentVelocity = 0f;

            if (Mathf.Abs(_currentVelocity) < _noMoveThresh)
            {
                _currentVelocity = 0f;
            }
            if (_currentVelocity > 0f)
            {
                _currentVelocity -= _decceleration * Time.deltaTime;
            } else if (_currentVelocity < 0f)
            {
                _currentVelocity += _decceleration * Time.deltaTime;
            }

        } else if (_upPressed)
        { 
            if (Mathf.Abs(_currentVelocity) < _maxSpeed)
            {
                _currentVelocity += _acceleration * Time.deltaTime;
            } else
            {
                _currentVelocity = _maxSpeed;
            }
        } else if (_downPressed)
        {
            if (Mathf.Abs(_currentVelocity) < _maxSpeed)
            {
                _currentVelocity -= _acceleration * Time.deltaTime;
            } else
            {
                _currentVelocity = -_maxSpeed;
            }
        }
    }

    private void MovePaddle()
    {
        if (_rigidbody2D == null) return;

        Vector2 v = _rigidbody2D.velocity;
        v.y = _currentVelocity;

        _rigidbody2D.velocity = v;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _touchingWall = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _touchingWall = false;
        }
    }

    public float GetWeightedSpeed()
    {
        return _speedTransfer * _currentVelocity;
    }


}

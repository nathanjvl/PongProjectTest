using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent<Vector2> OnDeathEvent;

    [Header("Ball Components")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Vector2 _velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetVelocity(Vector2 v)
    {
        _velocity = v;
    }


    // Update is called once per frame
    void Update()
    {
        Move();   
    }

    // handles the movement of the ball
    private void Move()
    {
        _rigidbody2D.velocity = _velocity * _speed;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("Collided with end!");
            SetVelocity(Vector2.zero);
            OnDie();
        } else if (collision.gameObject.CompareTag("Paddle"))
        {
            _velocity.x = -_velocity.x;

            Paddle paddle = collision.gameObject.GetComponent<Paddle>();
            if (paddle != null)
            {
                _velocity.y += paddle.GetWeightedSpeed();
            }

            Debug.Log("Collided with Paddle!");
        } else
        {
            _velocity.y = -_velocity.y;
        }
    }

    private void OnDie()
    {
        OnDeathEvent.Invoke(new Vector2(transform.position.x, transform.position.y));
        Destroy(gameObject);
    }
}

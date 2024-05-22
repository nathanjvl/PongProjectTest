using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent<Vector2> OnDeath;

    [SerializeField] private string _deathTag = "Death";
    [SerializeField] private string _paddleTag = "Paddle";

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
        _velocity.Normalize();
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
        if (collision.gameObject.CompareTag(_deathTag))
        {
            Debug.Log("Collided with end!");
            SetVelocity(Vector2.zero);
            OnDie();
        } else if (collision.gameObject.CompareTag(_paddleTag))
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
        OnDeath.Invoke(new Vector2(transform.position.x, transform.position.y));
        Destroy(gameObject);
    }
}

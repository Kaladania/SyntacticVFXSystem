using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _hp = 1;

    [SerializeField]
    Vector3 _targetPosition;

    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private float _minSpeed = 1;

    [SerializeField]
    private float _maxSpeed = 1;

    private Vector3 _movementDirection = Vector3.zero;

    bool destroyObject = false;

    //public GameObject EnemySpawner { get { return _targetPosition; } set { _targetPosition = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_targetPosition != null)
        {
            _movementDirection = (_targetPosition - transform.position).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _movementDirection * _speed;
    }

    private void LateUpdate()
    {
        if (destroyObject)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetMovementDirection(Vector3 destination)
    {
        _targetPosition = destination;
        _movementDirection = (_targetPosition - transform.position).normalized;
    }

    public void SetHP(int level)
    {
        level = level <= 0 ? 1 : level; //ensures min 'level' is 1 to not break calculations
        _hp = level;
        _speed = Mathf.Lerp(_minSpeed, _maxSpeed, 1 / level); //augments speed so strong enemy moves at max speed, weakest at min speed
    }

    private void OnCollisionEnter(Collision collision)
    {
        _speed = 0;
        destroyObject = true;
    }
}

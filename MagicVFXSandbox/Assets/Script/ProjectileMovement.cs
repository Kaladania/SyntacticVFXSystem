using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 20.0f;

    [SerializeField]
    private Vector3 _direction = Vector3.forward;

    public Vector3 Direction { get { return _direction; } set { _direction = value; } }

    private bool _destroyProjectile = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_projectileSpeed > 0)
        {
            Vector3 newPosition = transform.position; 
            newPosition += (_direction * (_projectileSpeed * Time.deltaTime));
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }
        else
        {
            Debug.Log("WARNING! Projectile is static. Speed is set to '0'");
        }
    }

    private void LateUpdate()
    {
        if (_destroyProjectile)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _projectileSpeed = 0;
        _destroyProjectile = true;
    }
}

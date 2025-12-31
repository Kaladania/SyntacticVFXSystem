using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 20.0f;

    [SerializeField]
    private Vector3 _direction = Vector3.forward;

    private Renderer _renderer;
    private BoxCollider _boxCollider;

    

    private bool _destroyProjectile = false;
    private int _targets = 0;

    public Vector3 Direction { get { return _direction; } set { _direction = value; } }
    public int Targets { get { return _targets; } set { _targets = value; } }

    public float Speed { get { return _projectileSpeed; } set { _projectileSpeed = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _boxCollider = GetComponent<BoxCollider>();
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

        //Destroy the object if there is no renderer or the object has gone offscreen
        if (_renderer != null && !_renderer.isVisible)
        {
            PrepareForDestruction();
        }
    }

    private void LateUpdate()
    {
        if (_destroyProjectile)
        {
            Destroy(this.gameObject);
        }
    }

    private void PrepareForDestruction()
    {
        if (_boxCollider != null)
        {
            _boxCollider.enabled = false; //disables collision so no more hits are registered
            _projectileSpeed = 0;
            _destroyProjectile = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _targets--;
        //only registers projectile for deletion if the collide object is the last target it can collide with
        if (_targets <= 0)
        {
            PrepareForDestruction();
        }
    }
}

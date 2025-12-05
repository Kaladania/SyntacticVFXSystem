using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_projectileSpeed > 0)
        {
            float newX = transform.position.x; 
            newX += (new Vector3(1, 0, 0) * (_projectileSpeed * Time.deltaTime)).x;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
        else
        {
            Debug.Log("WARNING! Projectile is static. Speed is set to '0'");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _projectileSpeed = 0;
        Destroy(this.gameObject);
    }
}

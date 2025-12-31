using UnityEngine;

public class RotateToMouse : MonoBehaviour
{

    [SerializeField]
    private float _maxRaycastLength = 0;

    [SerializeField]
    private GameObject _turret;

    [SerializeField]
    private Camera _cameraToLookAt;

    private Vector3 _direction;

    public Vector3 Direction { get { return _direction; } set { _direction = value; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray mouseRaycast;
        
        Vector3 mousePosition = Input.mousePosition; //gets the mouse screen position

        //raycasts to find the position the mouse is pointing to in world space
        mouseRaycast = _cameraToLookAt.ScreenPointToRay(mousePosition);

        //If there's a hit (mouse is hovering over game screen)
        if (_turret != null && _cameraToLookAt != null)
        {
            if (Physics.Raycast(mouseRaycast, out hit, _maxRaycastLength) )
            {
                //RotateToMouseDirection(_turret, hit.point);

                Vector3 direction = hit.point - transform.position;
                direction.y = 0; //prevents turrent from rotating on the Y axis

                if (direction != Vector3.zero)
                {
                    //calucates the quanternion rotation needed to get the turret to face the mouse
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);

                    _direction = direction;
                }
            }
           /* else
            {
                Vector3 position = mouseRaycast.GetPoint(_maxRaycastLength);
                //if turrent can't find a specific hit, just have it face the direction of a random world space point within range
                RotateToMouseDirection(_turret, position);
            }*/

            
        }
        else
        {
            Debug.Log("WARNING: Unable to rotate turret because either the turret or the camera is a null reference");
        }
    }

    //Rotates the turret in world space to face the mouse in screen space
    void RotateToMouseDirection(GameObject rotatingObject, Vector3 position)
    {
        //Quaternion newRotation = Quaternion.LookRotation(position - rotatingObject.transform.position);
        rotatingObject.transform.rotation = Quaternion.LookRotation(position - rotatingObject.transform.position); ;
        //rotatingObject.transform.localRotation = Quaternion.Lerp(rotatingObject.transform.rotation, newRotation, 1);
    }
}

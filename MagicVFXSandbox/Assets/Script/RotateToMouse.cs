using UnityEngine;

public class RotateToMouse : MonoBehaviour
{

    [SerializeField]
    private float _raycastLength = 0;

    [SerializeField]
    private GameObject _turret;

    [SerializeField]
    private Camera _cameraToLookAt;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (Physics.Raycast(mouseRaycast.origin, mouseRaycast.direction, out hit, _raycastLength) && _turret != null)
        {

            RotateToMouseDirection(_turret, hit.point);
        }
    }

    //Rotates the turret in world space to face the mouse in screen space
    void RotateToMouseDirection(GameObject rotatingObject, Vector3 position)
    {
        Quaternion newRotation = Quaternion.LookRotation(position - rotatingObject.transform.position);
        rotatingObject.transform.localRotation = Quaternion.Lerp(rotatingObject.transform.rotation,
            newRotation, 1);
    }
}

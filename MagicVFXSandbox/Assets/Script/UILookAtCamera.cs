using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    //Late update is called so that it runs AFTER all the major updates to prevent jittering
    private void LateUpdate()
    {
        //gets the UI element to face the direction of the camera
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}

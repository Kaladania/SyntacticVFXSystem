using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OrbAutoController : MonoBehaviour
{
   
    [SerializeField]
    protected Transform _spawnPoint = null; //holds the spawn point of the VFX projectiles

    [SerializeField]
    protected GameObject _projectile = null; //holds a prefab for a basic projectile

    [SerializeField]
    protected GameObject _childProjectile = null; //holds a prefab for a basic projectile

    /*[SerializeField]
    private DataRecorder.Recorder _dataRecorder = null;*/

    /*[SerializeField]
    private int _id = 0;*/

    void Start()
    {
        
        /*if (_dataRecorder == null)
        {
            Debug.LogError("ERROR! Data recorder reference is null. Creating a new runtime data recorder");

            _dataRecorder = new DataRecorder.Recorder();
        }*/

    }

    /// <summary>
    /// Combines the list of VFX systems to create a PCG VFX
    /// </summary>
    /// <param name="vfxToSpawn">The generated particle system to spawn</param>
    public GameObject CreateProjectile(List<VisualEffectAsset> generatedVFXs)
    {
        GameObject projectile = null;


        if (_spawnPoint != null && generatedVFXs != null)
        {
            Vector3 childSpawnPointPosition = _childProjectile.transform.position;
            projectile = Instantiate(_projectile, _spawnPoint.position, UnityEngine.Quaternion.identity);

            VisualEffect baseVfx = projectile.GetComponent<VisualEffect>();

            //Adds the base VFX for the head of the particle
           if (baseVfx == null)
           {
               Debug.LogError("WARNING! Failed to find Visual Effect Component");
           }
           else
           {
               //Adds the particle system to the loaded projectile prefab
               baseVfx.visualEffectAsset = generatedVFXs[0];
           }

            //rotate the projectile to face the turret facing direction
            projectile.transform.rotation = Quaternion.LookRotation(transform.forward);
            
            ProjectileMovement controller = projectile.GetComponent<ProjectileMovement>();

            if (controller != null)
            {
                controller.Direction = transform.right;
            }

            //loops through the rest of the array and adds the child VFX (trial + ambience)
            for (int i = 1; i < generatedVFXs.Count; i++)
            {
                GameObject childObject = Instantiate(_childProjectile, _spawnPoint.position + _childProjectile.transform.position, UnityEngine.Quaternion.identity);
                
                childObject.transform.parent = projectile.transform;
                //childObject.transform.localPosition = new Vector3(childPosition.x, childObject.transform.position.y, childObject.transform.position.z);

                VisualEffect childVFX = childObject.GetComponent<VisualEffect>();

                if (childVFX == null)
                {
                    Debug.LogError("WARNING! Failed to find Visual Effect Component");
                }
                else
                {
                    //Adds the particle system to the loaded projectile prefab
                    childVFX.visualEffectAsset = generatedVFXs[i];
                }
            }

        }

        return projectile;
    }

}

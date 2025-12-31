using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private Vector3 _spawnPosition = Vector3.zero; //center of spawn radius

    [SerializeField]
    private float _spawnRadius = 0; //spawn radius diameter

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private Transform _playerSpawnPoint;

    [SerializeField]
    private float _maxLevel = 1;

    [SerializeField]
    private float _spawnFrequency = 3.5f; //seconds

    [SerializeField]
    private float _maximumPositionSearchCount = 5; //number of times spawner will attempt to search for an empty spawn position

    private Vector3 currentSpawnPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_playerSpawnPoint == null)
        {
            _playerSpawnPoint = transform;
        }
        StartCoroutine(SpawnEnemy(_spawnFrequency, _enemyPrefab));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemy(float coundownDuration, GameObject enemy)
    {
        yield return new WaitForSeconds(coundownDuration); //returns a reference to the spawned enemy after a specified amount of time

        Vector3 spawnPosition = RandomPointOnCircleEdge(_spawnRadius);
        int level = (int)Random.Range(0, _maxLevel);


        //grabs a random spawn position on the circumference of a spawn radius circle
        /*Vector3 position = (Random.insideUnitCircle * _spawnRadius);
        position += transform.position;
        position.y = transform.position.y; //anchors object to the ground*/

        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        
        EnemyController controller = newEnemy.GetComponent<EnemyController>();

        //Sets up the enemy's movement direction and difficulty levels
        if (controller != null )
        {
            controller.SetMovementDirection(_playerSpawnPoint.position);
            controller.SetHP(level);
        }

        newEnemy.transform.rotation = Quaternion.LookRotation(newEnemy.transform.position - transform.position); //rotate the enemy to face the plauyer

        //randomises the scale ('difficulty') of an enemy to a random size
        int randomScale = 25 * level;
        newEnemy.transform.localScale *= randomScale == 0 ? 1 : 1 + ((float)randomScale / 100f); // /100 converts the int range to a float range (e.g. 25 -> 0.25)

        newEnemy.transform.Translate(new Vector3(0, newEnemy.transform.localScale.y / 2, 0)); //translates enemy upwards so it is sitting level with the floor

        StartCoroutine(SpawnEnemy(_spawnFrequency, _enemyPrefab));
    }


    private Vector3 RandomPointOnCircleEdge(float radius)
    {

        //bool goodSpawnLocation = false;

        Vector2 randomPosition = new Vector2(0, 0);

        /*while (_maximumPositionSearchCount > 0 && !goodSpawnLocation)
        {
            randomPosition = Random.insideUnitCircle.normalized * radius;

            //goodSpawnLocation = CheckIfPositionIsOcuppied(randomPosition);
            _maximumPositionSearchCount--;
            currentSpawnPosition = new Vector3(randomPosition.x, 0, randomPosition.y);
        }*/

        randomPosition = Random.insideUnitCircle.normalized * radius;

        //goodSpawnLocation = CheckIfPositionIsOcuppied(randomPosition);
        //_maximumPositionSearchCount--;
        currentSpawnPosition = new Vector3(randomPosition.x, 0, randomPosition.y);

        return new Vector3(randomPosition.x, 0, randomPosition.y);
    }

    /*private bool CheckIfPositionIsOcuppied(Vector2 vector2)
    {
        bool foundGoodLocation = true;

        Collider[] colliders = Physics.OverlapBox(vector2, new Vector3(1, 1, 1), Quaternion.identity);

        if (colliders.Length != 0)
        {
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    foundGoodLocation = false;
                }
            }
        }

        return foundGoodLocation;
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(currentSpawnPosition, new Vector3(1, 1, 1));
        }

        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public GameObject _enemyPrefab;
    public GameObject _rightEnemy;
    public GameObject _leftEnemy;
    public GameObject _enemyContainer;
    [SerializeField]
    private GameObject[]  powerups;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {



    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            Vector3 posToSpawnRight = new Vector3(-11.3f, Random.Range(-2, 7), 0);
            GameObject rightEnemy = Instantiate(_rightEnemy, posToSpawnRight, Quaternion.identity);
            rightEnemy.transform.parent = _enemyContainer.transform;

            Vector3 posToSpawnLeft = new Vector3(11.3f, Random.Range(-2, 7), 0);
            GameObject leftEnemy = Instantiate(_leftEnemy, posToSpawnLeft, Quaternion.identity);
            yield return new WaitForSeconds(6f);

        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        Destroy(this.gameObject);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 7);

            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6f, 9f));
          
           
        }
    }






}

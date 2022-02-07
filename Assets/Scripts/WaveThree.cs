using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveThree : MonoBehaviour
{
    public GameObject enemyOne;
    public GameObject _upRight;
    public GameObject _upLeft;
    public GameObject _diveBomb;
    [SerializeField] private GameObject[] spawnPoints;
    private Transform SpawnPoint;

    public GameObject _enemyContainer;
    [SerializeField]
    private Text _waveThreeText;
    // Start is called before the first frame update
    void Start()
    {

        SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();
        StartCoroutine(WaveOneSpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator WaveOneSpawnRoutine()
    {
        yield return new WaitForSeconds(2f);
        for (int flash = 0; flash < 3; flash++)
        {
            _waveThreeText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _waveThreeText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }

        for (int idx = 0; idx < 3F; idx++)
        {

            Vector3 newpos;
            SpawnPoint = spawnPoints[Random.Range(2, 6)].transform;
            newpos = SpawnPoint.position;
            Vector3 posToSpawn = new Vector3(newpos.x, newpos.y, 0);
            GameObject newEnemy = Instantiate(enemyOne, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2f);
            if (Random.Range(0, 101) < 50)
            {
                Vector3 newposUpRight;
                SpawnPoint = spawnPoints[Random.Range(0, 1)].transform;
                newposUpRight = SpawnPoint.position;
                Vector3 posToSpawnOnLeft = new Vector3(newposUpRight.x, newposUpRight.y, 0);
                GameObject newEnemyUpRight = Instantiate(_upRight, posToSpawnOnLeft, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            else
            {
                Vector3 newposUpLeft;
                SpawnPoint = spawnPoints[Random.Range(6, 7)].transform;
                newposUpLeft = SpawnPoint.position;
                Vector3 posToSpawnOnRight = new Vector3(newposUpLeft.x, newposUpLeft.y, 0);
                GameObject newEnemyUpLeft = Instantiate(_upLeft, posToSpawnOnRight, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(2f);
            Vector3 newposDiveBomb;
            SpawnPoint = spawnPoints[Random.Range(2, 6)].transform;
            newposDiveBomb = SpawnPoint.position;
            Vector3 posToSpawnDiveBomb = new Vector3(newposDiveBomb.x, newposDiveBomb.y, 0);
            GameObject newEnemyDiveBomb = Instantiate(_diveBomb, posToSpawnDiveBomb, Quaternion.identity);
            newEnemyDiveBomb.transform.parent = _enemyContainer.transform;

            Debug.LogError("inside the loop");
            yield return new WaitForSeconds(3f);
        }
    }
}

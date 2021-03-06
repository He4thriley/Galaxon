using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveOne : MonoBehaviour
{
    public GameObject enemyOne;
    [SerializeField] private GameObject[] spawnPoints;
    private Transform SpawnPoint;
   
    public GameObject _enemyContainer;
    [SerializeField]
    private Text _waveOneText;
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
            _waveOneText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _waveOneText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }

        for (int idx = 0; idx < 6F; idx++)
        { 
            
            Vector3 newpos;
            SpawnPoint = spawnPoints[Random.Range(2, 6)].transform;
            newpos = SpawnPoint.position;
            Vector3 posToSpawn = new Vector3(newpos.x, newpos.y, 0);
            GameObject newEnemy = Instantiate(enemyOne, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            Debug.LogError("inside the loop");
            yield return new WaitForSeconds(2f);
        }
       
    }
}

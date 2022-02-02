using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOne : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaveOneSpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
    }
}

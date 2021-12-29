using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    float _speed = 15f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.3f);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();


        }


    }

}

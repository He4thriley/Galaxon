using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3f;
    private float _canFire = -1;
    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(Random.Range(-8f, 8f), 7, 0);
        }

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
           GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }


        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
         
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.Score();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
       
        }
    }
}

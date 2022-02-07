using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private float _rotateSpeed = 50f;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3f;
    private float _canFire = 1;
    [SerializeField]
    private int enemyID;
    private bool isEnemyLaser;
    [SerializeField]
    private int _enemyShieldPercent;
    [SerializeField]
    private GameObject _enemyShield;
    private bool _shieldIsOnEnemy;
    [SerializeField]
    private float _playerDetectRange;
    public Rigidbody2D rigidBody;
    private bool _ramEnemyAlive;
    [SerializeField]
    private int _dodgeChancePercent;
    
    

    // Start is called before the first frame update
    void Start()
    {
      
        _ramEnemyAlive = true;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (Random.Range(1, 101) < _enemyShieldPercent)
        {
            _enemyShield.SetActive(true);
            _shieldIsOnEnemy = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
        switch(enemyID)
        {
            case 0:

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
            break;
            case 1:
                
                transform.Translate(Vector3.up * Time.deltaTime);
                    transform.Translate(Vector3.right * _speed * Time.deltaTime);
                if (transform.position.y > 9f)
                {
                    transform.position = new Vector3(-8f, Random.Range(-5f, 4f), 0);
                }

                if (Time.time > _canFire && transform.position.x > -9)
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
                break;
            case 2:
                
                transform.Translate(Vector3.up * Time.deltaTime);
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                if (transform.position.y > 9f)
                {
                    transform.position = new Vector3(8f, Random.Range(-5f, 4f), 0);
                }

                if (Time.time > _canFire && transform.position.x <9)
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
                break;
            case 3:
                RamPlayer();
              


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
                break;
            default:
                break;




        }

    }

    private void RamPlayer()
    {
        if (_ramEnemyAlive == true)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < _playerDetectRange)
            {


                Vector2 direction = (Vector2)_player.transform.position - rigidBody.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                if (rotateAmount < 0.5)
                {
                    rigidBody.angularVelocity = _rotateSpeed * rotateAmount;
                }
                // transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
                // transform.position = Vector3. * _speed * Time.deltaTime;
                Vector3 transformDown = transform.up * (-1);
                rigidBody.velocity = transformDown * _speed;

            }
            else
            {
                // transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
                transform.Translate(Vector3.down * _speed * Time.deltaTime); 
                if (transform.position.y < -5f)
                {
                    transform.position = new Vector3(Random.Range(-8f, 8f), 7, 0);
                }

            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {


        switch (enemyID)
        {
            case 0:
                if (other.tag == "Player")
                {
                    other.transform.GetComponent<Player>().Damage();
                    _anim.SetTrigger("OnEnemyDeath");
                    _player.Score();
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);

                }
                if (other.tag == "Laser")
                {

                    Destroy(other.gameObject);

                    if (_shieldIsOnEnemy == true)
                    {
                        _enemyShield.SetActive(false);
                        _shieldIsOnEnemy = false;
                        return;
                    }

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
                if (other.tag == "Beam")
                {
                    
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
                break;
            case 1:
                if (other.tag == "Player")
                {
                    other.transform.GetComponent<Player>().Damage();
                    _anim.SetTrigger("UpLeftDeath");
                    _player.Score();
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);

                }
                if (other.tag == "Laser")
                {

                    Destroy(other.gameObject);
                    if (_shieldIsOnEnemy == true)
                    {
                        _enemyShield.SetActive(false);
                        _shieldIsOnEnemy = false;
                        return;
                    }

                    if (_player != null)
                    {
                        _player.Score();
                    }
                    
                    _anim.SetTrigger("UpLeftDeath");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);

                }
                if (other.tag == "Beam")
                {
                    if (_shieldIsOnEnemy == true)
                    {
                        _enemyShield.SetActive(false);
                    }
                    if (_player != null)
                    {
                        _player.Score();
                    }
                    _anim.SetTrigger("UpLeftDeath");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);

                }
                break;
            case 2:
                if (other.tag == "Player")
                {
                    other.transform.GetComponent<Player>().Damage();
                    _anim.SetTrigger("UpLeftDeath");
                    _player.Score();
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);

                }
                if (other.tag == "Laser")
                {

                    Destroy(other.gameObject);

                    if (_shieldIsOnEnemy == true)
                    {
                        _enemyShield.SetActive(false);
                        _shieldIsOnEnemy = false;
                        return;
                    }

                    if (_player != null)
                    {
                        _player.Score();
                    }
                  
                    _anim.SetTrigger("UpLeftDeath");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);

                }
                if (other.tag == "Beam")
                {
                    if (_shieldIsOnEnemy == true)
                    {
                        _enemyShield.SetActive(false);
                    }
                    if (_player != null)
                    {
                        _player.Score();
                    }
                    _anim.SetTrigger("UpLeftDeath");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);

                }
                break;
            case 3:
                
                if (other.tag == "Player")
                {
                    _ramEnemyAlive = false;
                    other.transform.GetComponent<Player>().Damage();
                    _anim.SetTrigger("DivebombDeath");
                    _speed = 0;
                    _player.Score();
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 3f);

                }
                if (other.tag == "Laser")
                {
                    _ramEnemyAlive = false;
                    Destroy(other.gameObject);

                    if (_shieldIsOnEnemy == true)
                    {
                        _enemyShield.SetActive(false);
                        _shieldIsOnEnemy = false;
                        return;
                    }

                    if (_player != null)
                    {
                        _player.Score();
                    }

                    _anim.SetTrigger("DivebombDeath");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 3f);

                }
                if (other.tag == "Beam")
                {
                    _ramEnemyAlive = false;
                    if (_shieldIsOnEnemy == true)
                    {
                        _enemyShield.SetActive(false);
                    }
                    if (_player != null)
                    {
                        _player.Score();
                    }
                    _anim.SetTrigger("DivebombDeath");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 3f);

                }
                break;
            default:
                break;
        }

        }

    public void FireAtPowerup()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }
    public void FireUpAtPlayer()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].LaserUpAtPlayer();
        }


    }

    public void EnemyDodge()
    {
        if (Random.Range(1, 101) < _dodgeChancePercent)
        {
            int leftright = Random.Range(1, 101);
            if (leftright < 50)
            {
                transform.position += Vector3.left * Time.deltaTime * 40f;
                transform.position += Vector3.up * Time.deltaTime * 40f;
            }
            else
            {
                transform.position += Vector3.right * Time.deltaTime * 40f;
                transform.position += Vector3.up * Time.deltaTime * 40f;
            }
        }
    }

}

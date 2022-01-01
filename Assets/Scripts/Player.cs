using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject LaserPrefab;
    [SerializeField]
    float _speed = 3.5f;
    float _normSpeed = 3.5f;
    float _thrusterSpeed = 6f;
    float _speedMultiplier = 2f;
    public GameObject TriplePrefab;
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private float _canFire = -1.0f;
    private float _fireRate = 0.3f;
    private bool _tripleShotActive = false;
    private bool _speedActive = false;
    private bool _shieldsActive = false;
    private int _shieldLives;
    [SerializeField]
    private SpriteRenderer _shield;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightDamage;
    [SerializeField]
    private GameObject _leftDamage;
    [SerializeField]
    private AudioClip _laserSound;  
    private AudioSource _audioSource;

    
    // Start is called before the first frame update
    
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shield = GameObject.Find("Shield").GetComponent<SpriteRenderer>();



        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("the uimanager is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("the player audio source is NULL");
        }

        else {
            _audioSource.clip = _laserSound;
        }

    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _canFire < Time.time)
        {
            _canFire = Time.time + _fireRate;

            FireLaser();
        }





    }
    void playerMovement()
    {
        
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


            transform.Translate(direction * _speed * Time.deltaTime);
         

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    public void Damage ()
    {
        if (_shieldsActive == true)
        {
            if (_shieldLives == 2)
            {
                _shieldLives--;
                _shield.color = Color.magenta;
                return;
            }

            else if (_shieldLives == 1)
            {
                _shieldLives--;
                _shield.color = Color.red;
                return;
            }
            else
            {
                _shieldsActive = false;
                _shieldVisualizer.SetActive(false);
                return;
            }
        }
        _lives -= 1;

        _uiManager.UpdateLives(_lives);

        if (_lives == 3)
        {
            _rightDamage.SetActive(false);
            _leftDamage.SetActive(false);
        }

        else if (_lives == 2)
        {
            _rightDamage.SetActive(true);
            _leftDamage.SetActive(false);
        }
        else if (_lives == 1)
        {
            _leftDamage.SetActive(true);
        }



        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }
    }

    public void FireLaser ()
    {
        if (_tripleShotActive == true)
        {
            Instantiate(TriplePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(LaserPrefab, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speedActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerdown());
 
    }

    IEnumerator SpeedBoostPowerdown()
    {
        yield return new WaitForSeconds(5.0f);
        _speedActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _shieldsActive = true;
        _shieldLives = 2;
        _shieldVisualizer.SetActive(true);
        _shield.color = Color.white;
    }

    public void Score()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }

    public void ThrustersActive()
    {
        if (_speedActive == true)
        {
            return;
        }
        else
        {
            _speed = _thrusterSpeed;
        }
    }

    public void ThrustersInactive()
    {
        if (_speedActive == true)
        {
            return;
        }
        else
        {
            _speed = _normSpeed;
        }
    }


}

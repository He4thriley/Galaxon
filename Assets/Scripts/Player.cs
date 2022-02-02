using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool _beamActive = false;
    private bool _speedActive = false;
    private bool _shieldsActive = false;
    private int _shieldLives;
    [SerializeField]
    private SpriteRenderer _shield;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _beam;
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
    [SerializeField]
    private int _ammo;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField]
    private CameraShake _camera;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private EnemyHoming _deactivate;
    private bool playerDeath;
    public Thrusters _thrusters;


    // Start is called before the first frame update

    void Start()
    {
        playerDeath = false;
        _maxAmmo = 15;
        _ammo = 15;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shield = GameObject.Find("Shield").GetComponent<SpriteRenderer>();
        _beam = GameObject.Find("BeamPrefab");
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _thrusters = GameObject.Find("Thruster_Slider").GetComponent<Thrusters>();
        _deactivate = GameObject.Find("Diamond").GetComponent<EnemyHoming>();
      //  MaxAmmo();




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

        else
        {
            _audioSource.clip = _laserSound;
        }

    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        if (_beamActive == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canFire < Time.time)
            {
                _canFire = Time.time + _fireRate;
                FireLaser();
            }
        }
        else if (_beamActive == true) ;
        {

        }

        _uiManager.UpdateMaxAmmo(_maxAmmo);



    }
    void playerMovement()
    {


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);

        if (playerDeath == false)
        {
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
    }
    public void Damage()
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
        StartCoroutine(_camera.CameraShakeCoroutine(0.5f, 0.25f));

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
            playerDeath = true;
            transform.position = new Vector3(0f, 25f, 0f);
            _deactivate.playerExists = false;
            _spawnManager.OnPlayerDeath();



        }
    }

    public void FireLaser()
    {
        if (_ammo > 0)
        {
            if (_tripleShotActive == true)
            {
                Instantiate(TriplePrefab, transform.position, Quaternion.identity);

            }
            else
            {
                Instantiate(LaserPrefab, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);

            }
            AmmoShot();
            _audioSource.Play();
        }
    }
    public void RefillAmmo()
    {
        _ammo = _maxAmmo;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void HealthUp()
    {
        if (_lives < 3)
        {
            _lives += 1;
        }
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
    }

    public void AmmoShot()
    {
        _ammo--;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void MaxAmmo()
    {
      _uiManager.UpdateMaxAmmo(_maxAmmo);
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

    public void BeamActive()
    {
        _beamActive = true;
        _beam.SetActive(true);
        StartCoroutine(BeamPowerDown());
    }

    IEnumerator BeamPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _beamActive = false;
        _beam.SetActive(false);
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

    public void SlowActive()
    {
        _speedActive = true;
        _speed /= 2;
        StartCoroutine(SlowPowerdown());
        StartCoroutine(_thrusters.ThrustersPowerOff());
    }

    IEnumerator SlowPowerdown()
    {
        yield return new WaitForSeconds(4.0f);
        _speedActive = false;
        _speed *= 2;
    }
}

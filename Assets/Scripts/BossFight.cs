using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFight : MonoBehaviour
{
    [SerializeField]
    private CameraShake _camera;
    private Animator _anim;
    [SerializeField]
    private Animator _satelliteAnimator;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 2f;
    private float _laserBeamRate = 4f;
    private float _canFire = 1;
    private bool _bossCanShoot;
    public bool _shieldsOn;
    private float _speed;
    private AudioSource _audioSource;
    [SerializeField]
    private int _bossHit;
    private Player _player;
    public Rigidbody2D rigidBody;
    private SpriteRenderer _bossSprite;
    [SerializeField]
    private GameObject _bossExplodeObject;
    [SerializeField]
    private UIManager _uIManager;
    [SerializeField]
    private GameObject _bossLaser;
    [SerializeField]
    private Text _bossStartText;
    [SerializeField]
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _bossSprite = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _shieldsOn = true;
        _anim = GetComponent<Animator>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        StartCoroutine(BossEnterRoutine());
        StartCoroutine(BossLaserBeamRoutine());
        StartCoroutine(BossTextFlickerRoutine());
        _satelliteAnimator = GameObject.Find("Satellite").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _canFire && _bossCanShoot)
        {
            _fireRate = Random.Range(2f, 4f);
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
        }
        
        if (other.tag == "Laser")
        {
               
         
            if (_shieldsOn == true)
            {      
                return;
            }

            else if (_bossHit > 10)
            {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.Score();
                }
                _uIManager.BossBeatenUI();
                _anim.StopPlayback();
                _anim.SetTrigger("BossExplodes");      
                _speed = 0;
                _audioSource.Play();
                Destroy(_bossSprite);
                _spawnManager._stopSpawning = true;
                GameObject explode = Instantiate(_bossExplodeObject, transform.position, Quaternion.identity);
                rigidBody.velocity = Vector3.zero;
                Destroy(GetComponent<Collider2D>());
                _bossLaser.SetActive(false);
                Destroy(this.gameObject, 2.5f);
            }
            else
            {
                StartCoroutine(BossFlickerRoutine());
                _bossHit++;
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "Beam")
        {
            if (_shieldsOn == true)
            {
                return;
            }

            else if (_bossHit > 10)
            {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.Score();
                }
                _anim.StopPlayback();
                _anim.SetTrigger("BossExplodes");
                _speed = 0;
                _audioSource.Play();
                Destroy(_bossSprite);
                //_bossExplodeObject.SetActive(true);
                GameObject explode = Instantiate(_bossExplodeObject, transform.position, Quaternion.identity);
                rigidBody.velocity = Vector3.zero;
                _spawnManager._stopSpawning = true;
                Destroy(GetComponent<Collider2D>());
                _bossLaser.SetActive(false);
                Destroy(this.gameObject, 2.5f);
            }
            else
            {
                StartCoroutine(BossFlickerRoutine());
                _bossHit++;
                Destroy(other.gameObject);
                _player._beamActive = false;
            }


        }
    }

    IEnumerator BossEnterRoutine()
    {

        yield return new WaitForSeconds(3f);
        StartCoroutine(_camera.CameraShakeCoroutine(2.5f, 0.1f));
        _anim.SetTrigger("Enter");
        yield return new WaitForSeconds(1.2f);
        _anim.SetTrigger("PauseBeforeAttack");
        yield return new WaitForSeconds(1.2f);
        _anim.SetTrigger("BeginAttack");
        _satelliteAnimator.SetTrigger("BattleStarts");
        _bossCanShoot = true;

    }
    IEnumerator BossFlickerRoutine()
    {
        _bossSprite.color = Color.magenta;
        yield return new WaitForSeconds(0.1f);
        _bossSprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _bossSprite.color = Color.magenta;
        yield return new WaitForSeconds(0.1f);
        _bossSprite.color = Color.white;
    }

    IEnumerator BossLaserBeamRoutine()
    {
        yield return new WaitForSeconds(Random.Range(6f, 8f));
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 10f));
            _anim.enabled = false;
            _bossCanShoot = false;
            StartCoroutine(_camera.CameraShakeCoroutine(2.75f, 0.05f));
            yield return new WaitForSeconds(.5f);
            _bossLaser.SetActive(true);
            yield return new WaitForSeconds(2.25f);
            _anim.enabled = true;
            _bossCanShoot = true;
            _bossLaser.SetActive(false);
        }
    }
   IEnumerator BossTextFlickerRoutine()
    {
        for (int flash = 0; flash < 3; flash++)
        {
            _bossStartText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _bossStartText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }
}

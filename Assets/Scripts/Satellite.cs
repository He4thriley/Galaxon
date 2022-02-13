using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3f;
    private float _canFire = 1;
    private bool _satelliteCanFire;
    private Player _player;
    private AudioSource _audioSource;
    private float _speed;
    private Animator _anim;
    public int _satelliteHit;
    [SerializeField]
    private SpriteRenderer _satelliteSprite;
    [SerializeField]
    private GameObject _bossShield;
    [SerializeField]
    public BossFight _boss;
    
    [SerializeField]
    private GameObject _satelliteExplodeObject;


    // Start is called before the first frame update
    void Start()
    {
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossFight>();
        _satelliteSprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(WaitToFireDiamonds());
       _satelliteCanFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_satelliteCanFire == true)
        {
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(10f, 15f);
                _canFire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }


            }
        }
    }
    IEnumerator WaitToFireDiamonds()
    {
        yield return new WaitForSeconds(6f);
        _satelliteCanFire = true;
        yield return new WaitForSeconds(1f);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Laser")
        {
            _satelliteHit++;
            StartCoroutine(SatelliteFlickerRoutine());
            if (_satelliteHit == 2)
            {
                Destroy(other.gameObject);
              

                if (_player != null)
                {
                    _player.Score();
                }
               // _anim.SetTrigger("SatelliteExplodes");
                _bossShield.SetActive(false);
                Destroy(_satelliteSprite);
                GameObject explode = Instantiate(_satelliteExplodeObject, transform.position, Quaternion.identity);
                _speed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                _boss._shieldsOn = false;
                Destroy(this.gameObject, 2.5f);
            }
            else
            {
                Destroy(other.gameObject);
            }

        }
        if (other.tag == "Beam")
        {

            if (_player != null)
            {
                _player.Score();
            }
            _anim.SetTrigger("SatelliteExplodes");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);

        }
    }

    IEnumerator SatelliteFlickerRoutine()
    {
        _satelliteSprite.color = Color.magenta;
        yield return new WaitForSeconds(0.1f);
        _satelliteSprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _satelliteSprite.color = Color.magenta;
        yield return new WaitForSeconds(0.1f);
        _satelliteSprite.color = Color.white;
    }


}

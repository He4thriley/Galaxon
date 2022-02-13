using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 8f;
    private bool _isEnemyLaser = false;
    public bool _upEnemyLaser = false;
    // Start is called before the first frame update
    void Start()
    {
             _upEnemyLaser = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tag == "BossLaserBeam")
        {
            AssignEnemyLaser();
            return;
        }
        if (_isEnemyLaser == false || _upEnemyLaser == true)
        {
            MoveUp();
        }
       
        else
        {
            MoveDown();

        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);
        if (transform.position.y > 8)
        {
            Destroy(this.gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    public void LaserUpAtPlayer()
    {
        _upEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
        }
    }


}






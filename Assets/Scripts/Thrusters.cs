using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Thrusters : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private float _thrusterValue;
    [SerializeField]
    private bool _thrustersOn;
    [SerializeField]
    private UIManager _uIManager;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Image _thrusterImage;

    // Start is called before the first frame update
    void Start()
    {
        _thrusterSlider = GameObject.Find("Thruster_Slider").GetComponent<Slider>();
        _thrusterImage = GameObject.Find("Fill").GetComponent<Image>();
        _thrustersOn = true;
        _thrusterValue = 100f;
        _player = GetComponent<Player>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _thrusterSlider.image.color = Color.cyan;
        _thrusterImage.color = Color.cyan;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift) && _thrustersOn == true)
            
        {
            _player.ThrustersActive();
            ThrustersUse();
        }
        
        else
        {
            _player.ThrustersInactive();
            ThrustersGain();
        }

        _uIManager.ThrusterSliderValue(_thrusterValue);

    }

    public void ThrustersUse()
    {
       _thrusterValue -= 0.5f;
       if (_thrusterValue < 1)
       {
            _thrustersOn = false;
            StartCoroutine(ThrustersOff());
       }

    }

    public void ThrustersGain()
    {
        if (_thrusterValue < 100f)
        {
            _thrusterValue += 0.75f;
        }
    }


    public IEnumerator ThrustersOff()
    {

        _thrusterSlider.image.color = Color.red;
        _thrusterImage.color = Color.red;
        yield return new WaitForSeconds(1.7f);
        _thrustersOn = true;
        _thrusterSlider.image.color = Color.cyan;
        _thrusterImage.color = Color.cyan;

    }
    public IEnumerator ThrustersPowerOff()
    {
        _thrustersOn = false;
        _thrusterSlider.image.color = Color.red;
        _thrusterImage.color = Color.red;
        yield return new WaitForSeconds(4f);
        _thrustersOn = true;
        _thrusterSlider.image.color = Color.cyan;
        _thrusterImage.color = Color.cyan;

    }


}

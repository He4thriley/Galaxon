using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _livesIMG;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    bool _restartOkay = false;
    // Start is called before the first frame update
    void Start()
    {
        
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

            if (Input.GetKeyDown(KeyCode.R) && _restartOkay == true)
            {

                RestartLevel();
            }
    
            if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        

    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    
    public void UpdateLives(int currentLives)
    {
        _livesIMG.sprite = _livesSprite[currentLives];
        if (currentLives == 0)
        {
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
            _restartOkay = true;
            //StartCoroutine(RestartLevelRoutine());
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {

            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";

            yield return new WaitForSeconds(0.5f);
        }



    }


    public void RestartLevel()
    {

        SceneManager.LoadScene("Game");
        _restartOkay = false;
        
    }


}

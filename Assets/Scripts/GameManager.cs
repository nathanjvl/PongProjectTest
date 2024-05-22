using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("GameComponents")]
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _environmentCenter;
    [SerializeField] private TMP_Text _leftPointText;
    [SerializeField] private TMP_Text _rightPointText;
    [SerializeField] private GameObject _startGameMenu;

    [Header("Start Up Customization")]
    [SerializeField] private float _spawnCenterVariance = 3f;
    [SerializeField] private int _pointsToWin = 3;
    [SerializeField] private float _timeBetweenRounds = 2f;
    [SerializeField] private float _yVelocityVariance = 0.5f;

    private int _pointsRight = 0;
    private int _pointsLeft = 0;
    private bool _rightGotLastPoint = true;

    private bool _roundActive = false;
    private bool _gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_pointsToWin < 1) _pointsToWin = 1;  // catch oddcases
    }

    // Update is called once per frame
    void Update()
    {
        if (!_roundActive && _gameStarted)
        {

            StartCoroutine(StartRound());
        }
    }

    // called to start a new game of pong
    public void StartNewGame()
    {
        _startGameMenu?.SetActive(false);

        if (Random.Range(0f, 1f) > 0.5f)
        {
            _rightGotLastPoint = false;
        }

        SetPoints(0, 0);

        _roundActive = false;
        _gameStarted = true;
    }


    IEnumerator StartRound()
    {
        _roundActive = true;

        yield return new WaitForSeconds(_timeBetweenRounds);

        SpawnBall();
    }

    public void SpawnBall()
    {
        // Randomize Spawn location
        Vector3 spawnLocation = _environmentCenter.position;
        spawnLocation.y += Random.Range(-_spawnCenterVariance, _spawnCenterVariance);

        // Randomize Velocity
        Vector2 v = new Vector2(1f,Random.Range(-_yVelocityVariance, _yVelocityVariance));

        if (_rightGotLastPoint)
        {
            v.x = -1;
        }

        GameObject newBall = Instantiate(_ballPrefab, spawnLocation, Quaternion.identity);
        Ball ballScript = newBall.GetComponent<Ball>();
        if (ballScript != null)
        {
            ballScript.SetVelocity(v);
            ballScript.OnDeathEvent.AddListener(OnBallDeath);
        }
    }

    public void OnBallDeath(Vector2 pos)
    {
        if (_environmentCenter.position.x - pos.x < 0)
        {
            _rightGotLastPoint = false;
            
        } else
        {
            _rightGotLastPoint = true;
        }
        GainPoint(_rightGotLastPoint);

        if (_pointsLeft >= _pointsToWin) ShowWinner(false);
        else if (_pointsRight >= _pointsToWin) ShowWinner(true);

        _roundActive = false;
    }


    private void ShowWinner(bool isRightWinner)
    {
        _gameStarted = false;
        _startGameMenu?.SetActive(true);
    }

    #region Point Adjustment
    // sets the point values
    public void SetPoints(int pointsL, int pointsR)
    {
        _leftPointText.text = $"{pointsL}";
        _rightPointText.text = $"{pointsR}";
    }

    // adds 1 point
    public void GainPoint(bool isRight)
    {
        if (isRight)
        {
            _pointsRight += 1;
            // Effect here
        }
        else
        {
            _pointsLeft += 1;
            // Effect here
        }

        SetPoints(_pointsRight, _pointsLeft);
    }
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;

    // For move forward speed.
    public float runningSpeed;
    private float _currentRunningSpeed;

    // For x axis speed.
    public float limitX; 
    public float xSpeed; 

    // For stack mechanic.
    public List<GameObject> _positiveMoneys = new List<GameObject>();
    public GameObject stackPosition;

    // For score TMP.
    public TMP_Text _moneyScoreText = null;

    // For offset between stacking money.
    public float offsetMoney;

    // For gate number
    private int _gateNumber;

    // Money prefab for gates.
    public GameObject _moneyPrefab;

    // For negative gates.
    private int _targetScore;

    public Animator animator;

    
    public GameObject gameOverMenu;


    //deneme
    int currentLevel;
    public GameObject finishMenu;
    public Text finishScoreText;

    void Start()
    {
        //denemeeeeeee
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        if (SceneManager.GetActiveScene().name != "Level " + currentLevel)
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }

        Current = this;
        // _currentRunningSpeed = runningSpeed;

    }

    
    
    void Update()
    {
        if (LevelController.Current == null || !LevelController.Current.gameActive)
        {
            return;
        } 

     // Move forward and X axis.

        float newX = 0;
        float touchXDelta = 0;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchXDelta = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");
        }

        newX = transform.position.x + xSpeed * touchXDelta * Time.deltaTime;

        newX = Mathf.Clamp(newX, -limitX, limitX);

        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z + _currentRunningSpeed * Time.deltaTime);
        transform.position = newPosition;

        MoneyScoreText();

        StopGame();

        //deneme
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
     // Positive Money stack mechanic.
        if (other.gameObject.tag == "PositiveMoney")
        {
            other.gameObject.transform.SetParent(stackPosition.transform);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.localPosition = new Vector3(0f, _positiveMoneys[_positiveMoneys.Count - 1]. transform.localPosition.y + offsetMoney, 0f);
            _positiveMoneys.Add(other.gameObject);
            
        }
    // Negative Money destack mechanic.
        else if (other.gameObject.tag == "NegativeMoney")
        {

            other.gameObject.SetActive(false);
            _positiveMoneys[_positiveMoneys.Count - 1].transform.SetParent(null);
            _positiveMoneys[_positiveMoneys.Count - 1].SetActive(false);
            _positiveMoneys.Remove(_positiveMoneys[_positiveMoneys.Count - 1]);




        }

     // Gate mechanic.   
        if (other.gameObject.tag == "Gate")
        {
            other.gameObject.tag = "Untagged";
            _gateNumber = other.gameObject.GetComponent<GateController>().GetGateNumber();
            _targetScore = _positiveMoneys.Count + _gateNumber;
            if(_gateNumber > 0)
            {
                IncreaseMoney();
            }
            else if (_gateNumber < 0)
            {
                DecreaseMoney();
            }

        }

        if (other.gameObject.tag == "Finish")
        {
            FinishGame();
        }
    }

    private void MoneyScoreText()
    {
        _moneyScoreText.text = _positiveMoneys.Count.ToString();
        

       

    }

    private void IncreaseMoney()
    {
        for (int i = 0; i < _gateNumber; i++)
        {
            GameObject _newMoney = Instantiate(_moneyPrefab);
            _newMoney.transform.SetParent(stackPosition.transform);
            _newMoney.GetComponent<BoxCollider>().enabled = false;
            _newMoney.transform.localPosition = new Vector3(0f, _positiveMoneys[_positiveMoneys.Count - 1].transform.localPosition.y + offsetMoney, 0f);
            _positiveMoneys.Add(_newMoney.gameObject);
        }
    }

    private void DecreaseMoney()
    {
        for (int i = _positiveMoneys.Count -1 ; i >= _targetScore; i--)
        {
            _positiveMoneys[i].SetActive(false);
            _positiveMoneys.RemoveAt(i);
        }

    }

    private void StopGame()
    {
        if (_positiveMoneys.Count <= 0)
        {
            Time.timeScale = 0.0f;
            gameOverMenu.SetActive(true);
        }
    } 

    public void ChangeSpeed(float value)
    {
        _currentRunningSpeed = value;
    }


    //deneme

    public void FinishGame()
    {
        Time.timeScale = 0.0f;
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        finishScoreText.text = _moneyScoreText.text;
        finishMenu.SetActive(true);
        
    }


}

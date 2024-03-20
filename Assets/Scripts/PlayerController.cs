using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int count;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public AudioClip collectSound;
    public AudioSource audioSource;

    public float timeLeft = 30;
    public bool timerIsRunning = false;
    public TextMeshProUGUI TimeLeftText;

    private Rigidbody rb;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetGame();
        timerIsRunning = true;
    }

    void ResetGame()
    {
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        timeLeft = 90;
        timerIsRunning = true;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 40)
        {
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!";
            timerIsRunning = false;
            Invoke("LoadMainMenu", 5);  
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        if (transform.position.y < -10) // dead-end
        {
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            timerIsRunning = false;
            Invoke("LoadMainMenu", 3);
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
            audioSource.PlayOneShot(collectSound);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (count < 40) // dead-end
            {
                winTextObject.SetActive(true);
                winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
                timerIsRunning = false;
                Invoke("LoadMainMenu", 5); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timeLeft = 0;
                timerIsRunning = false;
                TimeLeftText.text = "Time's up!";
                if (count < 40)
                {
                    winTextObject.SetActive(true);
                    winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
                    Invoke("LoadMainMenu", 5); 
                }
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        TimeLeftText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

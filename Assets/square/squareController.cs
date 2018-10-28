using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class squareController : MonoBehaviour {
    public GameObject sSquare;
    public GameObject bSquare;
    public float dropSpeed;
    public float speed;
    public float speedup;
    public float scaleChange;
    public Text ScoreText;
    GameObject square1;
    GameObject square2;
    Vector3 posCheck;
    float timer;
    float score;
    bool gameOver;
    float gameoverTimer;

	// Use this for initialization
	void Start () {
        square1 = Instantiate(sSquare, new Vector3(0, 4, 0), Quaternion.identity);
        square2 = Instantiate(bSquare, new Vector3(0, -3.5f, 0), Quaternion.identity);
        square2.GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);

        score = 0;
        ScoreText.text = score.ToString();

        gameOver = false;
    }

    // Update is called once per frame
    void Update () {
        gameoverTimer += 1 * Time.deltaTime;

        if (gameOver == false)
        {
            ScoreText.text = score.ToString();

            if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)) && square1.transform.position.y >= 4)
            {
                square1.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -dropSpeed, 0);
            }

            if (square2.transform.position.x > 2.5)
            {
                square2.GetComponent<Rigidbody2D>().velocity = new Vector3(-square2.GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                square2.transform.position = new Vector3(2.5f, -3.5f, 0);
            }

            if (square2.transform.position.x < -2.5)
            {
                square2.GetComponent<Rigidbody2D>().velocity = new Vector3(-square2.GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                square2.transform.position = new Vector3(-2.5f, -3.5f, 0);
            }

            if (square1.transform.position.y < -10)
            {
                gameOver = true;
                ScoreText.text = "G  A  M  E \n" + score.ToString();
                gameoverTimer = 0;
            }

            if (squareCheck(square1, square2))
            {
                square1.transform.position = new Vector3(0, 4, 0);
                square1.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

                if (!(square2.GetComponent<Rigidbody2D>().velocity.x >= 7) && !(square2.GetComponent<Rigidbody2D>().velocity.x <= -7))
                {
                    if (square2.GetComponent<Rigidbody2D>().velocity.x > 0)
                    {
                        square2.GetComponent<Rigidbody2D>().velocity = new Vector3(square2.GetComponent<Rigidbody2D>().velocity.x + speedup, 0, 0);
                    }
                    else if (square2.GetComponent<Rigidbody2D>().velocity.x < 0)
                    {
                        square2.GetComponent<Rigidbody2D>().velocity = new Vector3(square2.GetComponent<Rigidbody2D>().velocity.x - speedup, 0, 0);
                    }
                }

                score++;
            }
        }

        if (gameOver && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)) && gameoverTimer > 1)
        {
            if (score > PlayerPrefs.GetFloat("sqHighScore"))
            {
                PlayerPrefs.SetFloat("sqHighScore", score);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene("menu");
        }
    }

    bool squareCheck(GameObject sq1, GameObject sq2)
    {
        float bounds = (sq2.transform.localScale.x / 2) + ((sq1.transform.localScale.x / 2) - 0.1f);

        if ((sq1.GetComponent<Rigidbody2D>().transform.position.x > sq2.transform.position.x - bounds && sq1.GetComponent<Rigidbody2D>().transform.position.x < sq2.transform.position.x + bounds) && (sq1.GetComponent<Rigidbody2D>().transform.position.y > sq2.transform.position.y - bounds && sq1.GetComponent<Rigidbody2D>().transform.position.y < sq2.transform.position.y + bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

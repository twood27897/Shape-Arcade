using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class circleController : MonoBehaviour {
    public GameObject circle;
    public GameObject targetCircle;
    public GameObject cover;
    public float expandSpeedUp;
    public float expandSpeed;
    public float targetMaxPos;
    public float targetMinPos;
    public float targetSize;
    public Text ScoreText;
    GameObject currentcircle;
    GameObject minAimCircle;
    GameObject maxAimCircle;
    Rigidbody2D circlerb;
    float score;
    float tMin;
    float tMax;
    bool gameOver;
    float gameoverTimer;

    //Start set up
    void Start() {

        gameOver = false;

        maxAimCircle = Instantiate(targetCircle, new Vector3(0, 0, 0), Quaternion.identity);
        minAimCircle = Instantiate(cover, new Vector3(0, 0, 0), Quaternion.identity);

        minAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(4, 4);
        tMin = 4;
        maxAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, 1);
        tMax = 4 + targetSize;

        currentcircle = Instantiate(circle, new Vector3(0, 0, 0), Quaternion.identity);
        circlerb = currentcircle.GetComponent<Rigidbody2D>();

        score = 0;
        ScoreText.text = score.ToString();

    }

    // Update is called once per frame
    void Update () {

        gameoverTimer += 1 * Time.deltaTime;

        if (!gameOver)
        {
            if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)) && (circlerb.transform.localScale.x > tMin && circlerb.transform.localScale.x < tMax))
            {
                circlerb.transform.localScale = new Vector3(0, 0, 1);
                TargetGen();
                score++;
                ScoreText.text = score.ToString();

                if (expandSpeed < 8)
                {
                    expandSpeed += expandSpeedUp;
                }
            }
            else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)))
            {
                gameOver = true;
                ScoreText.text = "G  A  M  E \n" + score.ToString();
                gameoverTimer = 0;
            }

            circlerb.transform.localScale = new Vector3(circlerb.transform.localScale.x + expandSpeed * Time.deltaTime, circlerb.transform.localScale.y + expandSpeed * Time.deltaTime, 1);
        }

        if (gameOver && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)) && gameoverTimer > 1)
        {
            if (score > PlayerPrefs.GetFloat("cirHighScore"))
            {
                PlayerPrefs.SetFloat("cirHighScore", score);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene("menu");
        }
    }

    void TargetGen()
    {
        float targetPos = Random.Range(targetMinPos, targetMaxPos);
        minAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(targetPos, targetPos);
        tMin = targetPos;

        maxAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, 1);
        tMax = targetPos + targetSize;
    }
}

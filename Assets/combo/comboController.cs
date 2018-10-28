using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class comboController : MonoBehaviour {
    public Camera cam;
    int gameChoice;
    bool changingGame;
    bool gameOver;
    float gameoverTimer;
    float score;
    public Text ScoreText;
    bool firstGame;

    Vector4 cir;
    Vector4 tr;
    Vector4 squ;
    Vector4 targetColour;
    public float colourSwitch;
    Color bg;
    float colourTimer;

    //triangle
    public int rotations;
    public float closeSpeed;
    public float speedUpTri;
    public GameObject controlTri;
    public GameObject target;
    GameObject rb;
    GameObject tri;

    //square
    public GameObject sSquare;
    public GameObject bSquare;
    public float dropSpeed;
    public float speed;
    public float speedUpSq;
    public float scaleChange;
    GameObject square1;
    GameObject square2;
    Vector3 posCheck;
    float timer;

    //circle
    public GameObject circle;
    public GameObject targetCircle;
    public GameObject cover;
    public float expandSpeedUp;
    public float expandSpeed;
    public float targetMaxPos;
    public float targetMinPos;
    public float targetSize;
    GameObject currentcircle;
    GameObject minAimCircle;
    GameObject maxAimCircle;
    Rigidbody2D circlerb;
    float tMin;
    float tMax;

    // Use this for initialization
    void Start()
    {
        firstGame = true;

        cir = new Vector4(0.780f, 0.180f, 0.478f, 1);
        tr = new Vector4(0.619f, 0.823f, 0.611f, 1);
        squ = new Vector4(0.588f, 0.866f, 0.882f, 1);

        if (PlayerPrefs.GetInt("comboChoice") == 0)
        {
            gameChoice = 1;
        }
        else if (PlayerPrefs.GetInt("comboChoice") == 1)
        {
            gameChoice = 2;
        }
        else if (PlayerPrefs.GetInt("comboChoice") == 2)
        {
            gameChoice = 0;
        }

        if (gameChoice == 0)
        {
            bg = tr;
        }
        else if (gameChoice == 1)
        {
            bg = squ;
        }
        else if (gameChoice == 2)
        {
            bg = cir;
        }
        cam.backgroundColor = bg;

        score = 0;
        changingGame = false;
        gameOver = false;
        colourTimer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        gameoverTimer += 1 * Time.deltaTime;
        colourTimer += 1 * Time.deltaTime;
        
        if (colourTimer < 2)
        {
            bg = Color.Lerp(cam.backgroundColor, targetColour, colourSwitch);
            cam.backgroundColor = bg;
        }
        else
        {
            cam.backgroundColor = targetColour;
        }

        if (!gameOver)
        {
            ScoreText.text = score.ToString();

            //triangle
            if (gameChoice == 0)
            {
                if (firstGame)
                {
                    targetColour = tr;
                    tri = Instantiate(target, new Vector3(0, 0, 0), Quaternion.identity);
                    tri.transform.localEulerAngles = new Vector3(0, 0, 40);
                    rb = Instantiate(controlTri, new Vector3(0, 0, 0), Quaternion.identity);
                    rb.transform.localEulerAngles = new Vector3(0, 0, 120);
                    firstGame = false;
                }
                else if (changingGame)
                {
                    targetColour = tr;
                    tri = Instantiate(target, new Vector3(0, 0, 0), Quaternion.identity);
                    tri.transform.localEulerAngles = new Vector3(0, 0, 120 / rotations);
                    rb = Instantiate(controlTri, new Vector3(0, 0, 0), Quaternion.identity);
                    rb.transform.localEulerAngles = new Vector3(0, 0, (120 / rotations) * Random.Range(1, rotations + 1));
                    changingGame = false;
                }
                if (tri.transform.localScale.x < 2 && rotationCheck())
                {
                    if (closeSpeed <= 7)
                    {
                        closeSpeed += speedUpTri;
                    }
                    
                    if (speed < 7)
                    {
                        speed += speedUpSq;
                    }

                    if (expandSpeed < 8)
                    {
                        expandSpeed += expandSpeedUp;
                    }

                    score++;

                    Destroy(tri);
                    Destroy(rb);

                    while (gameChoice == 0)
                    {
                        gameChoice = Random.Range(0, 3);
                    }
                    changingGame = true;
                    colourTimer = 0;
                }
                else if (tri.transform.localScale.x < 1 && !gameOver)
                {
                    gameOver = true;
                    gameoverTimer = 0;
                }
                else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
                {
                    rb.transform.localEulerAngles = new Vector3(0, 0, rb.transform.rotation.eulerAngles.z + (120 / rotations));
                }

                tri.transform.localScale = new Vector3(tri.transform.localScale.x - closeSpeed * Time.deltaTime, tri.transform.localScale.y - closeSpeed * Time.deltaTime, 1);

                if (rb.transform.localEulerAngles.z > 121)
                {
                    rb.transform.localEulerAngles = new Vector3(0, 0, 120 / rotations);
                }
            }
            //square
            else if (gameChoice == 1)
            {
                if (firstGame)
                {
                    targetColour = squ;
                    square1 = Instantiate(sSquare, new Vector3(0, 4, 0), Quaternion.identity);
                    square2 = Instantiate(bSquare, new Vector3(0, -3.5f, 0), Quaternion.identity);
                    square2.GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
                    firstGame = false;
                }
                else if (changingGame)
                {
                    targetColour = squ;
                    square1 = Instantiate(sSquare, new Vector3(0, 4, 0), Quaternion.identity);
                    square2 = Instantiate(bSquare, new Vector3(0, -3.5f, 0), Quaternion.identity);
                    square2.GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
                    changingGame = false;
                }

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
                    ScoreText.text = "GAME OVER \n" + score.ToString();
                    gameoverTimer = 0;
                }

                if (squareCheck(square1, square2))
                {
                    if (speed < 7)
                    {
                        speed += speedUpSq;
                    }

                    if (closeSpeed <= 7)
                    {
                        closeSpeed += speedUpTri;
                    }

                    if (expandSpeed < 8)
                    {
                        expandSpeed += expandSpeedUp;
                    }

                    score++;

                    Destroy(square1);
                    Destroy(square2);

                    while (gameChoice == 1)
                    {
                        gameChoice = Random.Range(0, 3);
                    }
                    changingGame = true;
                    colourTimer = 0;
                }
            }
            //circle
            else if (gameChoice == 2)
            {
                if (firstGame)
                {
                    targetColour = cir;
                    maxAimCircle = Instantiate(targetCircle, new Vector3(0, 0, 0), Quaternion.identity);
                    minAimCircle = Instantiate(cover, new Vector3(0, 0, 0), Quaternion.identity);
                    minAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(4, 4);
                    tMin = 4;
                    maxAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, 1);
                    tMax = 4 + targetSize;
                    currentcircle = Instantiate(circle, new Vector3(0, 0, 0), Quaternion.identity);
                    circlerb = currentcircle.GetComponent<Rigidbody2D>();
                    firstGame = false;
                }
                else if (changingGame)
                {
                    targetColour = cir;
                    maxAimCircle = Instantiate(targetCircle, new Vector3(0, 0, 0), Quaternion.identity);
                    minAimCircle = Instantiate(cover, new Vector3(0, 0, 0), Quaternion.identity);
                    TargetGen();
                    currentcircle = Instantiate(circle, new Vector3(0, 0, 0), Quaternion.identity);
                    circlerb = currentcircle.GetComponent<Rigidbody2D>();
                    changingGame = false;
                }

                minAimCircle.GetComponent<SpriteRenderer>().color = cam.backgroundColor;

                if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)) && (circlerb.transform.localScale.x > tMin && circlerb.transform.localScale.x < tMax))
                {
                    if (expandSpeed < 8)
                    {
                        expandSpeed += expandSpeedUp;
                    }

                    if (closeSpeed <= 7)
                    {
                        closeSpeed += speedUpTri;
                    }


                    if (speed < 7)
                    {
                        speed += speedUpSq;
                    }

                    score++;

                    Destroy(maxAimCircle);
                    Destroy(minAimCircle);
                    Destroy(currentcircle);

                    while (gameChoice == 2)
                    {
                        gameChoice = Random.Range(0, 3);
                    }
                    changingGame = true;
                    colourTimer = 0;
                }
                else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)))
                {
                    gameOver = true;
                    ScoreText.text = "GAME OVER \n" + score.ToString();
                    gameoverTimer = 0;
                }

                circlerb.transform.localScale = new Vector3(circlerb.transform.localScale.x + expandSpeed * Time.deltaTime, circlerb.transform.localScale.y + expandSpeed * Time.deltaTime, 1);
            }
        }

        if (gameOver)
        {
            ScoreText.text = "G  A  M  E" + "\n" + score.ToString();
        }
        if (gameOver && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && gameoverTimer > 1)
        {
            if (score > PlayerPrefs.GetFloat("comboHighScore"))
            {
                PlayerPrefs.SetFloat("comboHighScore", score);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene("menu");
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    bool rotationCheck()
    {
        if ((rb.transform.localEulerAngles.z - tri.transform.localEulerAngles.z) > -1 && (rb.transform.localEulerAngles.z - tri.transform.localEulerAngles.z) < 1)
        {
            return true;
        }
        else
        {
            return false;
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

    void TargetGen()
    {
        float targetPos = Random.Range(targetMinPos, targetMaxPos);
        minAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(targetPos, targetPos);
        tMin = targetPos;

        maxAimCircle.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, minAimCircle.GetComponent<Rigidbody2D>().transform.localScale.x + targetSize, 1);
        tMax = targetPos + targetSize;
    }
}

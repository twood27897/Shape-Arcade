using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour {
    public Camera cam;
    public GameObject OuterCircle;
    public GameObject CoverCircle;
    public GameObject BigTri;
    public GameObject SmallTri;
    public GameObject BigSq;
    public GameObject SmallSq;
    public GameObject square;
    public GameObject combo;
    public float switchSpeed;
    public float animSpeed;
    static int choice = 0;
    Vector3 cameraMove;
    Vector3 target;
    bool moving;
    Vector4 cir;
    Vector4 tri;
    Vector4 squ;
    Vector4 triangleColour;
    Vector4 targetColor;
    Color bg;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    public Text ScoreText;
    int anim;
    float startTimer;

    // Use this for initialization
    void Start () {
        triangleColour = new Vector4(0.639f, 0.741f, 0.769f, 1);

        cir = new Vector4(0.780f, 0.180f, 0.478f, 1);
        tri = new Vector4(0.619f, 0.823f, 0.611f, 1);
        squ = new Vector4(0.588f, 0.866f, 0.882f, 1);

        if (choice == 3)
        {
            if (PlayerPrefs.GetInt("comboChoice") == 0)
            {
                choice = 0;
            }
            else if (PlayerPrefs.GetInt("comboChoice") == 1)
            {
                choice = 1;
            }
            else if (PlayerPrefs.GetInt("comboChoice") == 2)
            {
                choice = 2;
            }
        }

        if (choice == 0)
        {
            anim = 0;
            bg = cir;
            cam.transform.position = new Vector3(0, 0, -10);
            PlayerPrefs.SetInt("comboChoice", 0);

            ScoreText.color = new Vector4(0.95f, 0.32f, 0.47f, 1);
            ScoreText.text = PlayerPrefs.GetFloat("cirHighScore").ToString();
        }
        else if (choice == 1)
        {
            anim = 1;
            bg = tri;
            cam.transform.position = new Vector3(18, 0, -10);
            PlayerPrefs.SetInt("comboChoice", 1);

            ScoreText.color = new Vector4(0.64f, 0.74f, 0.75f, 1);
            ScoreText.text = PlayerPrefs.GetFloat("triHighScore").ToString();
        }
        else if (choice == 2)
        {
            anim = 2;
            bg = squ;
            cam.transform.position = new Vector3(36, 0, -10);
            PlayerPrefs.SetInt("comboChoice", 2);

            ScoreText.color = new Vector4(0.18f, 0.39f, 0.62f, 1);
            ScoreText.text = PlayerPrefs.GetFloat("sqHighScore").ToString();
        }
        cam.backgroundColor = bg;

        if (choice != 1)
        {
            BigTri.GetComponent<SpriteRenderer>().color = cam.backgroundColor;
        }
        else if (choice == 1)
        {
            BigTri.GetComponent<SpriteRenderer>().color = triangleColour;
        }

        PlayerPrefs.Save();

        startTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        startTimer += Time.deltaTime;

        if (startTimer > 0.5)
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    firstPressPos = t.position;
                }
                if (t.phase == TouchPhase.Ended)
                {
                    secondPressPos = t.position;

                    currentSwipe = secondPressPos - firstPressPos;
                    currentSwipe.Normalize();

                    if (currentSwipe.x < -0.5f && (currentSwipe.y < 0.5f && currentSwipe.y > -0.5f))
                    {
                        if (choice == 0)
                        {
                            anim++;
                            choice++;
                            moving = true;
                            target = BigTri.transform.position;
                            targetColor = tri;
                        }
                        else if (choice == 1)
                        {
                            anim++;
                            choice++;
                            moving = true;
                            target = square.transform.position;
                            targetColor = squ;
                        }

                        startTimer = 0;
                    }
                    else if (currentSwipe.x > 0.5f && (currentSwipe.y < 0.5f && currentSwipe.y > -0.5f))
                    {
                        if (choice == 1)
                        {
                            anim--;
                            choice--;
                            moving = true;
                            target = CoverCircle.transform.position;
                            targetColor = cir;
                        }
                        else if (choice == 2)
                        {
                            anim--;
                            choice--;
                            moving = true;
                            target = BigTri.transform.position;
                            targetColor = tri;
                        }

                        startTimer = 0;
                    }
                    else if ((currentSwipe.y > 0.5f && (currentSwipe.x < 0.5f && currentSwipe.x > -0.5f)) && !moving)
                    {
                        if (choice == 0)
                        {
                            PlayerPrefs.SetInt("comboChoice", 0);
                        }
                        else if (choice == 1)
                        {
                            PlayerPrefs.SetInt("comboChoice", 1);
                        }
                        else if (choice == 2)
                        {
                            PlayerPrefs.SetInt("comboChoice", 2);
                        }
                        PlayerPrefs.Save();

                        moving = true;
                        choice = 3;
                        target = new Vector3(cam.transform.position.x, -10, -10);

                        startTimer = 0;
                    }
                    else
                    {
                        if (choice == 0)
                        {
                            SceneManager.LoadScene("circlegame");
                        }
                        else if (choice == 1)
                        {
                            SceneManager.LoadScene("trianglegame");
                        }
                        else if (choice == 2)
                        {
                            SceneManager.LoadScene("squaregame");
                        }
                    }
                }
            }
        }


            if (moving && transform.position != target)
            {
                ScoreText.text = "";

                if (choice != 1)
                {
                    BigTri.GetComponent<SpriteRenderer>().color = Color.Lerp(BigTri.GetComponent<SpriteRenderer>().color, targetColor, switchSpeed);

                    if ((transform.position.x - target.x) < 0.05 && (transform.position.x - target.x) > -0.05)
                    {
                        BigTri.GetComponent<SpriteRenderer>().color = targetColor;
                    }
                }
                else if (choice == 1)
                {
                    BigTri.GetComponent<SpriteRenderer>().color = Color.Lerp(BigTri.GetComponent<SpriteRenderer>().color, triangleColour, switchSpeed);

                    if ((transform.position.x - target.x) < 0.05 && (transform.position.x - target.x) > -0.05)
                    {
                        BigTri.GetComponent<SpriteRenderer>().color = triangleColour;
                    }
                }

                cameraMove = Vector3.Lerp(transform.position, target, switchSpeed);
                cameraMove.z = -10;
                transform.position = cameraMove;

                bg = Color.Lerp(cam.backgroundColor, targetColor, switchSpeed);
                cam.backgroundColor = bg;

                if (choice == 3)
                {
                    if ((transform.position.y - target.y) < 0.05 && (transform.position.y - target.y) > -0.05)
                    {
                        transform.position = new Vector3(target.x, target.y, -10);
                        moving = false;
                        SceneManager.LoadScene("combogame");
                    }
                }
                else if ((transform.position.x - target.x) < 0.05 && (transform.position.x - target.x) > -0.05)
                {
                    transform.position = new Vector3(target.x, target.y, -10);
                    Camera.main.backgroundColor = targetColor;
                    moving = false;

                    if (choice == 0)
                    {
                        ScoreText.color = new Vector4(0.95f, 0.32f, 0.47f, 1);
                        ScoreText.text = PlayerPrefs.GetFloat("cirHighScore").ToString();
                    }
                    else if (choice == 1)
                    {
                        ScoreText.color = new Vector4(0.64f, 0.74f, 0.75f, 1);
                        ScoreText.text = PlayerPrefs.GetFloat("triHighScore").ToString();
                    }
                    else if (choice == 2)
                    {
                        ScoreText.color = new Vector4(0.18f, 0.39f, 0.62f, 1);
                        ScoreText.text = PlayerPrefs.GetFloat("sqHighScore").ToString();
                    }
                }

                CoverCircle.GetComponent<SpriteRenderer>().color = cam.backgroundColor;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (choice == 0)
                {
                    anim++;
                    choice++;
                    moving = true;
                    target = BigTri.transform.position;
                    targetColor = tri;
                }
                else if (choice == 1)
                {
                    anim++;
                    choice++;
                    moving = true;
                    target = square.transform.position;
                    targetColor = squ;
                }
                else if (choice == 2)
                {
                    anim = 0;
                    choice = 0;
                    moving = true;
                    target = CoverCircle.transform.position;
                    targetColor = cir;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (choice == 0)
                {
                    anim = 2;
                    choice = 2;
                    moving = true;
                    target = square.transform.position;
                    targetColor = squ;
                }
                else if (choice == 1)
                {
                    anim--;
                    choice--;
                    moving = true;
                    target = CoverCircle.transform.position;
                    targetColor = cir;
                }
                else if (choice == 2)
                {
                    anim--;
                    choice--;
                    moving = true;
                    target = BigTri.transform.position;
                    targetColor = tri;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && !moving)
            {
                if (choice == 0)
                {
                    PlayerPrefs.SetInt("comboChoice", 0);
                }
                else if (choice == 1)
                {
                    PlayerPrefs.SetInt("comboChoice", 1);
                }
                else if (choice == 2)
                {
                    PlayerPrefs.SetInt("comboChoice", 2);
                }
                PlayerPrefs.Save();

                moving = true;
                choice = 3;
                target = new Vector3(cam.transform.position.x, -10, -10);
            }

            if (anim == 0)
            {
                if (OuterCircle.transform.localScale.x <= 4.75)
                {
                    animSpeed *= -1;
                }
                else if (OuterCircle.transform.localScale.x >= 5.25)
                {
                    animSpeed *= -1;
                }

                OuterCircle.transform.localScale = new Vector3(OuterCircle.transform.localScale.x + (animSpeed * Time.deltaTime), OuterCircle.transform.localScale.y + (animSpeed * Time.deltaTime), OuterCircle.transform.localScale.z);
                CoverCircle.transform.localScale = new Vector3(CoverCircle.transform.localScale.x + (animSpeed * Time.deltaTime), CoverCircle.transform.localScale.y + (animSpeed * Time.deltaTime), CoverCircle.transform.localScale.z);
            }
            else if (anim == 1)
            {
                if (BigTri.transform.localScale.x <= 9.8)
                {
                    animSpeed *= -1;
                }
                else if (BigTri.transform.localScale.x >= 10.2)
                {
                    animSpeed *= -1;
                }

                BigTri.transform.localScale = new Vector3(BigTri.transform.localScale.x + (animSpeed * Time.deltaTime), BigTri.transform.localScale.y + (animSpeed * Time.deltaTime), BigTri.transform.localScale.z);
                SmallTri.transform.localScale = new Vector3(SmallTri.transform.localScale.x + (animSpeed * Time.deltaTime), SmallTri.transform.localScale.y + (animSpeed * Time.deltaTime), SmallTri.transform.localScale.z);
            }
            else if (anim == 2)
            {
                if (BigSq.transform.localScale.x <= 0.9)
                {
                    animSpeed *= -1;
                }
                else if (BigSq.transform.localScale.x >= 1.1)
                {
                    animSpeed *= -1;
                }

                BigSq.transform.localScale = new Vector3(BigSq.transform.localScale.x + (animSpeed * Time.deltaTime), BigSq.transform.localScale.y + (animSpeed * Time.deltaTime), BigSq.transform.localScale.z);
                SmallSq.transform.localScale = new Vector3(SmallSq.transform.localScale.x + (animSpeed * Time.deltaTime), SmallSq.transform.localScale.y + (animSpeed * Time.deltaTime), SmallSq.transform.localScale.z);
            }

            if (choice == 0 && Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("circlegame");
            }
            else if (choice == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("trianglegame");
            }
            else if (choice == 2 && Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("squaregame");
            }
    }
}

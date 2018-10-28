using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class triRotation : MonoBehaviour {
    public int rotations;
    public float closeSpeed;
    public float speedUp;
    public GameObject controlTri;
    public GameObject target;
    public Text text;
    GameObject rb;
    GameObject tri;
    bool gameOver;
    float score;
    float gameoverTimer;
    
	// Use this for initialization
	void Start () {
        tri = Instantiate(target, new Vector3(0, 0, 0), Quaternion.identity);
        tri.transform.localEulerAngles = new Vector3(0, 0, 40);

        rb = Instantiate(controlTri, new Vector3(0, 0, 0), Quaternion.identity);
        rb.transform.localEulerAngles = new Vector3(0, 0, 120);

        score = 0;

        gameOver = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

        gameoverTimer += 1 * Time.deltaTime;

        if (!gameOver)
        {
            if (tri.transform.localScale.x < 2 && rotationCheck())
            {
                if (closeSpeed <= 7)
                {
                    closeSpeed += speedUp;
                }

                tri.transform.localScale = new Vector3(10, 10, 0);
                tri.transform.localEulerAngles = new Vector3(0, 0, (120 / rotations) * Random.Range(1, rotations + 1));

                rb.transform.localEulerAngles = new Vector3(0, 0, 120 / rotations);

                score++;
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

            text.text = score.ToString();
        }
        if (gameOver)
        {
            text.text = "G  A  M  E" + "\n" + score.ToString();
        }
        if (gameOver && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && gameoverTimer > 1)
        {
            if (score > PlayerPrefs.GetFloat("triHighScore"))
            {
                PlayerPrefs.SetFloat("triHighScore", score);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene("menu");
        }
    }

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int maxSize;
    public int currentSize;
    public int xBound;
    public int yBound;
    public int score;
    public GameObject foodPrefab;
    public GameObject currentFood;
    public GameObject snakePrefab;
    public Snake head;
    public Snake tail;
    public int NEWS;
    public Vector2 nextPos;
    public Text scoreText;
    public float deltaTimer;

    // Use this for initialization
    void OnEnable()
    {
        Snake.hit += hit;
    }
    void Start ()
    {
        InvokeRepeating("TimerInvoke", 0, deltaTimer);
        FoodFunction();
	}

    void OnDisable()
    {
        Snake.hit -= hit;
    }

    // Update is called once per frame
    void Update ()
    {
        ComChangeD();
	}

    void TimerInvoke()
    {
        Movement();
        StartCoroutine(CheckVisable());
        if(currentSize >= maxSize)
        {
            TailFunction();
        }
        else
        {
            currentSize++;
        }
    }

    void Movement()
    {
        GameObject temp;
        nextPos = head.transform.position;

        switch (NEWS)
        {
            case 0:
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1:
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2:
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3:
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;
        }

        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);

        head.Setnext(temp.GetComponent<Snake>());
        head = temp.GetComponent<Snake>();

        return;
    }

    void ComChangeD()
    {
        if(NEWS!=2 && Input.GetKeyDown(KeyCode.W))
        {
            NEWS = 0;
        }
        if (NEWS != 3 && Input.GetKeyDown(KeyCode.D))
        {
            NEWS = 1;
        }
        if (NEWS != 0 && Input.GetKeyDown(KeyCode.S))
        {
            NEWS = 2;
        }
        if (NEWS != 1 && Input.GetKeyDown(KeyCode.A))
        {
            NEWS = 3;
        }
    }

    void TailFunction()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        StartCoroutine(CheckRender(currentFood)); 

    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if(IN.GetComponent<Renderer>().isVisible == false)
        {
            if(IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunction();
            }
        }
    }

    void hit(string WhatWasSent)
    {
        if(WhatWasSent=="Food")
        {
            if(deltaTimer >= .1f)
            {
                deltaTimer -= .05f;
                CancelInvoke("TimerInvoke");
                InvokeRepeating("TimerInvoke", 0, deltaTimer);
            }    
            FoodFunction();
            maxSize++;
            score++;
            scoreText.text = score.ToString();
            int temp = PlayerPrefs.GetInt("HighScore");
            if(score > temp)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        if(WhatWasSent == "Snake")
        {
            CancelInvoke("TimerInvoke");
            Exit();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    void Wrap()
    {
        if(NEWS == 0)
        {
            head.transform.position = new Vector2(head.transform.position.x, -(head.transform.position.y - 1));
        }
        else if (NEWS == 1)
        {
            head.transform.position = new Vector2(-(head.transform.position.x - 1), head.transform.position.y);
        }
        else if (NEWS == 2)
        {
            head.transform.position = new Vector2(head.transform.position.x, -(head.transform.position.y + 1));
        }
        else if (NEWS == 3)
        {
            head.transform.position = new Vector2(-(head.transform.position.x + 1), head.transform.position.y);
        }
    }

    IEnumerator CheckVisable()
    {
        yield return new WaitForEndOfFrame();
        if(!head.GetComponent<Renderer>().isVisible)
        {
            Wrap();
        }
         
    }
}

using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{
    public static GameController instance;
    public static int ticker;

    [SerializeField] GameObject fillPrefabs;
    [SerializeField] Cell[] allCells;

    public static Action<String> slide;
    public float slideTimer = 0;
    public int score;
    [SerializeField] Text scoreDisplay;
    public Color[] fillCollers;

    int isGameOver;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] int winCon;
    [SerializeField] GameObject GameWinPanel;
    bool haswon;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        StartSpawnFill();
        StartSpawnFill();
    }

    // Update is called once per frame
    void Update()
    {
        if (slideTimer >= 0.3f && !GameWinPanel.activeInHierarchy && !gameOverPanel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ticker = 0;
                isGameOver = 0;
                slide("w");
                slideTimer = 0;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                ticker = 0;
                isGameOver = 0;
                slide("a");
                slideTimer = 0;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ticker = 0;
                isGameOver = 0;
                slide("s");
                slideTimer = 0;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                ticker = 0;
                isGameOver = 0;
                slide("d");
                slideTimer = 0;
            }
        }
        slideTimer += Time.deltaTime;
    }

    public void SpawnFill()
    {
        bool isFull = true;
        for (int i =0; i < allCells.Length; i++)
        {
            if (allCells[i].fill ==  null)
            {
                isFull = false;
            }
        }

        if (isFull == true)
        {
            return;
        }

        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);
        if (allCells[whichSpawn].transform.childCount != 0)
        {
            SpawnFill();
            return;
        }

        float chance = UnityEngine.Random.Range(0.0f, 1.0f);
        Debug.Log(chance);

        if (chance < 0.2f)
        {
            return;
        }
        else if (chance < 0.8f)
        {
            GameObject tempfill = Instantiate(fillPrefabs, allCells[whichSpawn].transform);
            Debug.Log(2);
            Fill tempFillComp = tempfill.GetComponent<Fill>();
            allCells[whichSpawn].GetComponent<Cell>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(2);
        }
        else
        {
            GameObject tempfill = Instantiate(fillPrefabs, allCells[whichSpawn].transform);
            Debug.Log(4);
            Fill tempFillComp = tempfill.GetComponent<Fill>();
            allCells[whichSpawn].GetComponent<Cell>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(4);
        }
    }


    public void StartSpawnFill()
    {
        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);
        if (allCells[whichSpawn].transform.childCount != 0)
        {
            SpawnFill();
            return;
        }
        GameObject tempfill = Instantiate(fillPrefabs, allCells[whichSpawn].transform);
        Debug.Log(2);
        Fill tempFillComp = tempfill.GetComponent<Fill>();
        allCells[whichSpawn].GetComponent<Cell>().fill = tempFillComp;
        tempFillComp.FillValueUpdate(2);
     
    }

    public void ScoreUpdate(int sc)
    {
        score += sc;
        scoreDisplay.text = score.ToString();
    }

    public void GameOverCheck()
    {
        isGameOver++;
        if (isGameOver >= 16)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }


    public void WinCheck(int higestFill)
    {
        if (haswon)
            return;
        if (higestFill == winCon)
        {
            GameWinPanel.SetActive(true);
            haswon = true;
        }
    }

    public void KeepPlaying()
    {
        GameWinPanel.SetActive(false);
    }
}

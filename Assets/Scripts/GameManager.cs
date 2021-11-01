using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }

    public int ghostMultiplier { get; private set; } = 1;

    public TextMeshProUGUI scoreText;
    public Image[] livesCounterImages;
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI readyText;
    public TextMeshProUGUI gameOverText;

    public bool startedGame { get; private set; } = false;

    private void Start()
    {
        waitingText.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!startedGame && Input.anyKeyDown)
        {
            PressedStartButton();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        for(int i = 0; i < livesCounterImages.Length; i++)
        {
            livesCounterImages[i].enabled = true;
        }
        NewRound();
    }

    private void NewRound()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        StartCoroutine(StartReset());
        
    }

    private void ResetState()
    {
        ResetGhostMultiplier();
        foreach (Ghost ghost in ghosts)
        {
            ghost.ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }
        pacman.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        startedGame = false;
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        if(lives < 3 && lives > 0)
        {
            livesCounterImages[livesCounterImages.Length - lives].enabled = false;
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 4f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        foreach(Ghost ghost in ghosts)
        {
            ghost.frightened.Enable(pellet.duration);
        }
        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);

    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
    }

    public void PacmanCaught()
    {
        Time.timeScale = 0f;
        SetLives(lives - 1);
        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }
        StartCoroutine(RoundLost());
    }

    public void PressedStartButton()
    {
        if (!startedGame)
        {
            gameOverText.gameObject.SetActive(false);
            startedGame = true;
            waitingText.gameObject.SetActive(false);
            NewGame();
        }
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    private IEnumerator StartReset()
    {
        ResetState();
        Time.timeScale = 0f;
        readyText.gameObject.SetActive(true);
        //TODO: play ready sfx
        yield return new WaitForSecondsRealtime(4f);//sfx duration
        readyText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator RoundLost()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        //TODO: play death sfx
        pacman.DeathAnimation();
        yield return new WaitForSecondsRealtime(2f);
        if (lives > 0)
        {
            StartCoroutine(StartReset());
        }
        else
        {
            GameOver();
        }
        
    }
}

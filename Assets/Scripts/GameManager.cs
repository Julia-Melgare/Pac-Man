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
    public TextMeshProUGUI bonusScoreText;
    public Image[] livesCounterImages;
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI readyText;
    public TextMeshProUGUI gameOverText;

    public AudioManager audioManager { get; private set; }

    public bool startedGame { get; private set; } = false;

    private void Start()
    {
        audioManager = AudioManager.instance;
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
        audioManager.backgroundSource.Stop();
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
        audioManager.PlayBackgroundNoise(audioManager.powerPelletClip);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
        Invoke(nameof(ResetBackgroundNoise), pellet.duration);
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
        StartCoroutine(GhostEatenAnimation(ghost, points));

    }

    public void PacmanCaught()
    {
        CancelInvoke();
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

    private void ResetBackgroundNoise()
    {
        audioManager.PlayBackgroundNoise(audioManager.sirenClip);
    }

    private IEnumerator StartReset()
    {
        CancelInvoke();
        ResetState();
        Time.timeScale = 0f;
        readyText.gameObject.SetActive(true);
        audioManager.PlaySoundEffect(audioManager.gameStartClip);
        yield return new WaitForSecondsRealtime(4f);
        readyText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        ResetBackgroundNoise();
    }

    private IEnumerator RoundLost()
    {
        audioManager.backgroundSource.Stop();
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        audioManager.PlaySoundEffect(audioManager.pacmanDeathClip);
        pacman.DeathAnimation();
        yield return new WaitForSecondsRealtime(3f);
        if (lives > 0)
        {
            StartCoroutine(StartReset());
        }
        else
        {
            GameOver();
        }        
    }

    private IEnumerator GhostEatenAnimation(Ghost ghost, int points)
    {
        Time.timeScale = 0f;
        //TODO: play sfx
        pacman.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        bonusScoreText.text = points.ToString();
        bonusScoreText.transform.position = Camera.main.WorldToScreenPoint(ghost.gameObject.transform.position + Vector3.left);
        bonusScoreText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        bonusScoreText.gameObject.SetActive(false);
        pacman.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Time.timeScale = 1f;
    }
}

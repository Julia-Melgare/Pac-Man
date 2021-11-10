using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public GameObject fruitPrefab;
    private GameObject fruit;

    public int score { get; private set; }
    public int lives { get; private set; }
    public int highScore { get; private set; }

    public int ghostMultiplier { get; private set; } = 1;

    private int pelletsEaten = 0;

    public AnimationManager animationManager;
    public AudioClip gameStartClip;
    public AudioClip levelWinClip;

    public bool startedGame { get; private set; } = false;

    private void Start()
    {
        Time.timeScale = 0f;
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if (!startedGame && Input.anyKeyDown)
        {
            PressedStartButton();
        }
    }
#endif

    private void NewGame()
    {
        SetScore(0);
        SetLives(6);
        animationManager.ResetLivesCounter();
        LevelManager.instance.ResetState();
        animationManager.ResetState();
        NewRound();
    }

    private void NewRound()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        pelletsEaten = 0;
        LevelManager.instance.NextLevel();
        animationManager.NextLevel();
        SetPacmanLevelParameters();
        SetGhostsLevelParameters();
        StartCoroutine(StartReset());
    }

    private void ResetState()
    {
        AudioManager.instance.backgroundSource.Stop();
        if (fruit != null) Destroy(fruit);
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
        animationManager.gameOverText.gameObject.SetActive(true);
        startedGame = false;
    }

    private void SetScore(int score)
    {
        this.score = score;
        animationManager.SetScoreUI(score.ToString());
        SaveHighScore();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        animationManager.UpdateLivesCounter(lives);
    }

    private void SetPacmanLevelParameters()
    {
        pacman.movement.speed = LevelManager.instance.pacmanSpeed;
        pacman.movement.speedMultiplier = 1f;
    }

    private void SetGhostsLevelParameters()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.movement.speed = LevelManager.instance.ghostsSpeed;
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);
        pelletsEaten++;
        if (!HasRemainingPellets())
        {
            StartCoroutine(RoundWon());
        }
        else if (pelletsEaten == 70 || pelletsEaten == 170) //fruits spawn when 70 pellets and 170 pellets have been eaten
        {
            StartCoroutine(SpawnFruit());
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        pellet.duration = LevelManager.instance.frightDuration;
        foreach (Ghost ghost in ghosts)
        {
            if (!ghost.frightened.eaten)
            {
                ghost.frightened.Enable(pellet.duration);
            }            
        }
        PelletEaten(pellet);
        pacman.movement.speedMultiplier = LevelManager.instance.frightPacmanSpeed / LevelManager.instance.pacmanSpeed;
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
        Invoke(nameof(ResetBackgroundNoise), pellet.duration);
        Invoke(nameof(SetPacmanLevelParameters), pellet.duration);
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier*=2;
        StartCoroutine(animationManager.GhostEatenAnimation(ghost, points));
    }

    public void FruitEaten(Fruit fruit)
    {
        fruit.gameObject.SetActive(false);
        SetScore(score + fruit.points);
        StartCoroutine(animationManager.FruitEatenAnimation(fruit));

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
            animationManager.gameOverText.gameObject.SetActive(false);
            startedGame = true;
            animationManager.waitingText.gameObject.SetActive(false);
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

    private void SaveHighScore()
    {
        if(score >= PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
            animationManager.UpdateHighScoreUI();
        }
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    private void ResetBackgroundNoise()
    {
        AudioManager.instance.PlayBackgroundNoise(ghosts[0].sirenClip);
    }

    private IEnumerator StartReset()
    {
        CancelInvoke();
        ResetState();
        Time.timeScale = 0f;
        animationManager.readyText.gameObject.SetActive(true);
        AudioManager.instance.PlaySoundEffect(gameStartClip);
        yield return new WaitForSecondsRealtime(4f);
        animationManager.readyText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        ResetBackgroundNoise();
    }

    private IEnumerator RoundLost()
    {
        AudioManager.instance.backgroundSource.Stop();
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        AudioManager.instance.PlaySoundEffect(pacman.pacmanDeathClip);
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

    private IEnumerator RoundWon()
    {
        Time.timeScale = 0f;
        AudioManager.instance.backgroundSource.Stop();
        pacman.gameObject.SetActive(false);
        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }
        yield return new WaitForSecondsRealtime(1f);
        AudioManager.instance.backgroundSource.PlayOneShot(levelWinClip);
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        NewRound();
    }

    private IEnumerator SpawnFruit()
    {
        fruit = Instantiate(fruitPrefab);
        fruit.GetComponent<SpriteRenderer>().sprite = LevelManager.instance.fruitSprite;
        fruit.GetComponent<Fruit>().points = LevelManager.instance.fruitPoints;
        fruit.SetActive(true);
        yield return new WaitForSeconds(Random.Range(9f, 10f)); //fruits stay active from 9 to 10 seconds
        if(fruit != null) fruit.SetActive(false);
    }
}

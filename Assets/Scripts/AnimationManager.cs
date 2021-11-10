using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    public Pacman pacman;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI bonusScoreText;
    public TextMeshProUGUI fruitScoreText;
    public Image[] livesCounterImages;
    public Image[] levelCounterImages;
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI readyText;
    public TextMeshProUGUI gameOverText;

    private int levelCounterImagesIndex = 0;

    private void Start()
    {
        waitingText.gameObject.SetActive(true);
        UpdateHighScoreUI();
    }

    public void ResetLivesCounter()
    {
        for (int i = 0; i < livesCounterImages.Length; i++)
        {
            livesCounterImages[i].enabled = true;
        }
    }

    public void SetScoreUI(string score)
    {
        scoreText.text = score;
    }

    public void UpdateLivesCounter(int lives)
    {
        if (lives <= livesCounterImages.Length && lives > 0)
        {
            livesCounterImages[livesCounterImages.Length - lives].enabled = false;
        }
    }

    public void NextLevel()
    {
        levelText.text = LevelManager.instance.level.ToString();
        levelCounterImages[levelCounterImagesIndex].sprite = LevelManager.instance.fruitSprite;
        levelCounterImages[levelCounterImagesIndex].gameObject.SetActive(true);
        levelCounterImagesIndex++;
        if(levelCounterImagesIndex >= levelCounterImages.Length)
        {
            levelCounterImagesIndex = 0;
        }
    }

    public void ResetState()
    {
        for (int i = 0; i < levelCounterImages.Length; i++)
        {
            levelCounterImages[i].gameObject.SetActive(false);
            levelCounterImages[i].sprite = null;
        }
        levelCounterImagesIndex = 0;
        UpdateHighScoreUI();
    }

    public void UpdateHighScoreUI()
    {
        highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
    }

    public IEnumerator GhostEatenAnimation(Ghost ghost, int points)
    {
        Time.timeScale = 0f;
        AudioManager.instance.backgroundSource.Stop();
        AudioManager.instance.PlaySoundEffect(ghost.eatGhostClip);
        pacman.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        bonusScoreText.text = points.ToString();
        bonusScoreText.transform.position = Camera.main.WorldToScreenPoint(ghost.gameObject.transform.position + Vector3.left);
        bonusScoreText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        bonusScoreText.gameObject.SetActive(false);
        pacman.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Time.timeScale = 1f;
        AudioManager.instance.PlayBackgroundNoise(ghost.ghostRetreatingClip);
    }

    public IEnumerator FruitEatenAnimation(Fruit fruit)
    {
        fruitScoreText.text = fruit.points.ToString();
        fruitScoreText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        fruitScoreText.gameObject.SetActive(false);
    }
}

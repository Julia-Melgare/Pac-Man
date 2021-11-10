using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int level { get; private set; }
    public Sprite[] fruitSprites;
    public Sprite fruitSprite { get; private set; }
    public int fruitPoints { get; private set; }
    public float pacmanSpeed { get; private set; }
    public float ghostsSpeed { get; private set; }
    public float ghostsTunnelSpeed { get; private set; }
    public float frightPacmanSpeed { get; private set; }
    public float frightGhostsSpeed { get; private set; }
    public float frightDuration { get; private set; }

    public static LevelManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        level = 0;
    }

    public void ResetState()
    {
        level = 0;
    }

    public void NextLevel()
    //Set game parameters for each level according to those in the original Pac-Man: https://www.gamasutra.com/db_area/images/feature/3938/tablea1.png
    {
        level++;
        switch (level)
        {
            case 1:
                fruitSprite = fruitSprites[0];
                fruitPoints = 100;
                pacmanSpeed = 8f;
                frightPacmanSpeed = 9f;                
                frightDuration = 6f;
                break;
            case 2:
                fruitSprite = fruitSprites[1];
                fruitPoints = 300;
                pacmanSpeed = 9f;
                frightPacmanSpeed = 9.5f;
                frightDuration = 5f;
                break;
            case 3:
                fruitSprite = fruitSprites[2];
                fruitPoints = 500;
                frightDuration = 4f;
                break;
            case 4:
                frightDuration = 3f;
                break;
            case 5:
                fruitSprite = fruitSprites[3];
                fruitPoints = 700;
                pacmanSpeed = 10f;
                frightPacmanSpeed = 10f;
                frightDuration = 2f;
                break;
            case 6:
                frightDuration = 5f;
                break;
            case var _ when level >=7 && level <= 8:
                fruitSprite = fruitSprites[4];
                fruitPoints = 1000;
                frightDuration = 2f;
                break;
            case 9:
                fruitSprite = fruitSprites[5];
                fruitPoints = 2000;
                frightDuration = 1f;
                break;
            case 10:
                frightDuration = 5f;
                break;
            case 11:
                fruitSprite = fruitSprites[6];
                fruitPoints = 3000;
                frightDuration = 2f;
                break;
            case 12:
                frightDuration = 1f;
                break;
            case 13:
                fruitSprite = fruitSprites[7];
                fruitPoints = 5000;
                break;
            case 14:
                frightDuration = 3f;
                break;
            case var _ when level >= 15 && level < 21:
                frightDuration = 1f;
                break;
        }
        if(level < 21)
        {
            ghostsSpeed = pacmanSpeed - 0.5f;
            ghostsTunnelSpeed = pacmanSpeed / 2f;
            frightGhostsSpeed = ghostsTunnelSpeed + 1f;
        }
        else
        {
            pacmanSpeed = 9f;
            ghostsSpeed = 9.5f;
            ghostsTunnelSpeed = 5f;

        }
        
    }
}

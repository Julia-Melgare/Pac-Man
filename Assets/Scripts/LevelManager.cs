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
    public float frightPacmanSpeed { get; private set; }
    public float frightGhostsSpeed { get; private set; }
    public float frightDuration { get; private set; }

    private void Awake()
    {
        level = 0;
    }

    public void NextLevel()
    {
        level++;
        switch (level)
        {
            case 1:
                fruitSprite = fruitSprites[0];
                fruitPoints = 100;
                pacmanSpeed = 8f;
                ghostsSpeed = 7.5f;
                frightPacmanSpeed = 9f;
                frightGhostsSpeed = 5f;
                frightDuration = 6f;
                break;
            case 2:
                fruitSprite = fruitSprites[1];
                fruitPoints = 300;
                pacmanSpeed = 9f;
                ghostsSpeed = 8.5f;
                frightPacmanSpeed = 9.5f;
                frightGhostsSpeed = 5.5f;
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
                ghostsSpeed = 9.5f;
                frightPacmanSpeed = 10f;
                frightGhostsSpeed = 6f;
                frightDuration = 2f;
                break;
        }
    }
}

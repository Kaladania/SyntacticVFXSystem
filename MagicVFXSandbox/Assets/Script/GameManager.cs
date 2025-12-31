using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameStates state;

    private void Awake()
    {
        if (instance != this)
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = GameStates.IN_GAME;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameStates.IN_GAME:
                break;
            case GameStates.END_GAME:
                break;
            default:
                break;
        }
    }

    void UpdateGameState(GameStates newState)
    {
        if (newState != state)
        {
            state = newState;
            switch (newState)
            {
                case GameStates.IN_GAME:
                    break;
                case GameStates.END_GAME:
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), newState, null);
            }
        }
    }
}

public enum GameStates
{
    IN_GAME, 
    END_GAME
}
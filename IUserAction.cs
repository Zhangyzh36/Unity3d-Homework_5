using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { IN_ROUND, OUT_ROUND, IN_GAME, START }
public enum ActionMode { PHYSICS, KINEMATIC, NONE }

public interface IUserAction
{
    void GameOver();
    void hit(Vector3 pos);
    ActionMode getMode();
    void setMode(ActionMode m);
    GameState getGameState();
    void setGameState(GameState gs);
    int GetScore();
}
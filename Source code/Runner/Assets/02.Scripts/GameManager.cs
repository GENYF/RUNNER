using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject startUI, ingameUI, gameoverUI;

    public Text bestScoreText_Start, bestScoreText_Over;
    public Text scoreText_Over;
    public Text calorieText_Over;

    public ObstacleManager obstacleManager;
    public SerialManager serialManager;
    public MapManager mapManager;
    public PlayerCtrl playerCtrl;

    public static GameManager instance;

    private void Awake()
    {
        Time.timeScale = 1;
           instance = this;
        bestScoreText_Start.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }

    public void StartGame()
    {
        startUI.SetActive(false);
        gameoverUI.SetActive(false);
        ingameUI.SetActive(true);

        obstacleManager.ResetAll();
        playerCtrl.ResetAll();
        mapManager.isStop = serialManager.isStop = false;
    }

    public void GameOver(int score, float cal)
    {
        mapManager.isStop = serialManager.isStop = true;
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScore = score;
        }
        bestScoreText_Over.text = bestScore.ToString();
        scoreText_Over.text = score.ToString();
        calorieText_Over.text = cal.ToString("F1") + "kcal";

        startUI.SetActive(false);
        ingameUI.SetActive(false);
        gameoverUI.SetActive(true);
    }
}

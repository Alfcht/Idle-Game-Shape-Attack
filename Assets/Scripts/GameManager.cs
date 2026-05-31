using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public int ChosenShape = 0;
    public Color ChosenColor = Color.cyan;
    public string ChosenAttribute = "explosion";

    public int Gold = 0;
    public int WaveNumber = 1;
    public int EnemiesLeft = 0;
    public float TowerHP = 100f;
    public float TowerMaxHP = 100f;

    public int LvlDamage = 1;
    public int LvlSpeed = 1;
    public int LvlHP = 1;
    public int LvlCritChance = 1;
    public int LvlCritDamage = 1;
    public int LvlAttribute = 1;

    public void AddGold(int Amount) => Gold += Amount;

    public bool SpendGold(int Amount)
    {
        if (Gold < Amount) return false;
        Gold -= Amount;
        return true;
    }

    public void StartGame() => SceneManager.LoadScene("Game");

    public void GoToMenu() => SceneManager.LoadScene("MainMenu");
}
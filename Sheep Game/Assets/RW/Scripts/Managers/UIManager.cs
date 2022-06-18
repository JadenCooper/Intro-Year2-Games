using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text sheepSavedText;
    public Text sheepDroppedText;
    public GameObject gameOverWindow;

    public TextAsset file;
    public Text Highscore;
    private string PreviousHighScore;
    private string path;
    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        path = $"{Application.dataPath}/RW/TextFiles/Highscore.txt";
        AssignHighScore();
    }

    public void UpdateSheepSaved()
    {
        sheepSavedText.text = GameStateManager.Instance.sheepSaved.ToString();
    }

    public void UpdateSheepDropped()
    {
        sheepDroppedText.text = GameStateManager.Instance.sheepDropped.ToString();
    }
    public void ShowGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }

    public void AssignHighScore()
    {
        List<string> temp = File.ReadAllLines(path).ToList();
        foreach (string line in temp)
        {
            PreviousHighScore = line;
        }
        Highscore.text = PreviousHighScore;
    }
    public void SetHighScore()
    {
        if ( int.Parse(sheepSavedText.text)  > int.Parse(PreviousHighScore))
        {
            File.WriteAllText(path, sheepSavedText.text);
        }
    }
}

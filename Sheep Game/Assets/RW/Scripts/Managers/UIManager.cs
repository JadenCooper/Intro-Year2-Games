using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    // This Script Manage's The UI

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
        // Sets the current score
        sheepSavedText.text = GameStateManager.Instance.sheepSaved.ToString();
    }

    public void UpdateSheepDropped()
    {
        // Sets the lives
        sheepDroppedText.text = GameStateManager.Instance.sheepDropped.ToString();
    }
    public void ShowGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }

    public void AssignHighScore()
    {
        // This method pulls in the previous highscore from the highscore text file then assigns it
        List<string> temp = File.ReadAllLines(path).ToList();
        foreach (string line in temp)
        {
            PreviousHighScore = line;
        }
        Highscore.text = PreviousHighScore;
    }
    public void SetHighScore()
    {
        // This method checks if the current game's new highscore is bigger than the previous, if so rewrites the text file with it
        if ( int.Parse(sheepSavedText.text)  > int.Parse(PreviousHighScore))
        {
            File.WriteAllText(path, sheepSavedText.text);
        }
    }
}

using MVVM;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [Data("Score")]
    [SerializeField] public  TMP_Text _scoreText;
}

using UnityEngine;

[CreateAssetMenu(fileName = "Questions List", menuName = "Datas/Questions List")]
public class QuestionListSo : ScriptableObject
{
    public QuestionSo[] allQuestions;
}
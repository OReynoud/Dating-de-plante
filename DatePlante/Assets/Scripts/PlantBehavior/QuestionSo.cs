using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Datas/Plant Question")]
public class QuestionSo : ScriptableObject
{
    public ThemesList questionTheme;
    public QuestionTypes questionType;
    [ResizableTextArea] public string questionContent;
}

using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public QuestionData questionData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindFirstObjectByType<QuizManager>().ShowQuestion(questionData);


            collision.GetComponent<PlayerController>().StopMoving();
        }
    }
}
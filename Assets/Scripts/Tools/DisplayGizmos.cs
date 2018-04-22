using UnityEngine;

public class DisplayGizmos : MonoBehaviour
{
    GameObject target;
    Vector3 direction;
    Color lineColor;
    string behaveStatement;
    string lenght;

    public void DisplayDrawLine(string behave, GameObject obj, Color color, string lineLenght)
    {
        target = obj;
        lineColor = color;
        behaveStatement = behave;
        lenght = lineLenght;
    }

    void Update()
    {
        switch (behaveStatement)
        {
            case "Look":
                direction = new Vector3(0f, 0f, 0f);
                break;
            case "Chase":
                direction = Vector3.Normalize(target.transform.position - transform.position);
                transform.position += direction * .1f;
                break;
            default:
                print("Nothing");
                break;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        switch (lenght)
        {
            case "Short":
                Gizmos.DrawLine(transform.position, transform.position + direction);
                break;
            case "Long":
                Gizmos.DrawLine(transform.position, target.transform.position);
                break;
            default:
                print("Nothing");
                break;
        }
    }
}

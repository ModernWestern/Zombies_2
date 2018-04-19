using UnityEngine;

public class DisplayGizmos : MonoBehaviour
{
    GameObject target;

    public void Gizmo(GameObject obj)
    {
        target = obj;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target.transform.position);
    }
}

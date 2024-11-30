using UnityEngine;

public class PathScript : MonoBehaviour
{
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        var distance = (Camera.main.transform.position - transform.position).magnitude;
        if (distance > 2000)
        {
            var size = distance * 0.00001f * Camera.main.fieldOfView;
            transform.localScale = new Vector3(size, transform.localScale.y, size);
        }
    }

    private Quaternion offset = Quaternion.Euler(-90, 0, 0);

    public void setPoints(Vector3 p1, Vector3 p2)
    {
        var delta = p2 - p1;

        // First apply the world space rotation so the forward vector points p1 to p2
        transform.rotation = Quaternion.LookRotation(delta);
        // Then add the local offset around the local X axis
        transform.localRotation *= offset;
        // Or also
        //cylinder.Rotate(-90, 0, 0);

        // Set the position to the center between p1 and p2
        transform.position = (p1 + p2) / 2f;

        // Set the Y scale to the half of the distance between p1 and p2    
        var scale = transform.localScale;
        scale.y = delta.magnitude /* / 2 */;
        transform.localScale = scale;
    }

    public void setColor(Color color)
    {
        renderer.material.SetColor("_Color", color);
    }

    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }
}

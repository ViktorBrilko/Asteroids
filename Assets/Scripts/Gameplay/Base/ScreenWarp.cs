using Gameplay.Gamefields;
using UnityEngine;
using Zenject;

public class ScreenWarp : MonoBehaviour
{
    private GameField _field;

    [Inject]
    public void Construct(GameField field)
    {
        _field = field;
    }
    
    public void Warp()
    {
        Vector3 position = transform.position;
        Bounds bounds = _field.GetComponent<Collider2D>().bounds;

        if (position.x > bounds.max.x)
            transform.position = new Vector3(-transform.position.x, transform.position.y);
        else if (position.x < bounds.min.x)
            transform.position = new Vector3(-transform.position.x, transform.position.y);
        else if (position.y > bounds.max.y)
            transform.position = new Vector3(transform.position.x, -transform.position.y);
        else if (position.y < bounds.min.y)
            transform.position = new Vector3(transform.position.x, -transform.position.y);
    }
}
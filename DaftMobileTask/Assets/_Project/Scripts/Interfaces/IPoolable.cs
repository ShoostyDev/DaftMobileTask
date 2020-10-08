using UnityEngine;

public interface IPoolable
{
    bool IsAvailable();

    void Recycle();
    void Deactivate();
    void Activate(Vector2 pos);
    Vector2 GetPosition();
}

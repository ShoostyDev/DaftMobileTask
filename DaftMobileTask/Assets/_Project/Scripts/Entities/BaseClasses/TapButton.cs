using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class TapButton : MonoBehaviour, IPoolable
{
    protected float LifeTime = 4;
    protected float LifeTimer;
    protected bool IsActive;

    [SerializeField]
    protected GameObject ExplosionParticle;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonTap);
    }

    private void Start()
    {
        StartCoroutine(LifeTimeCounter());
    }

    protected IEnumerator LifeTimeCounter()
    {
        while (LifeTimer < LifeTime)
        {
            LifeTimer += Time.deltaTime;
            yield return null;
        }

        Deactivate();
    }

    protected abstract void OnButtonTap();

    public bool IsAvailable()
    {
        return !IsActive;
    }

    public void Activate(Vector2 pos)
    {
        IsActive = true;
        gameObject.SetActive(IsActive);
        transform.position = pos;
    }

    public void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(IsActive);
    }

    public void Recycle()
    {
        StartCoroutine(LifeTimeCounter());

    }

    public Vector2 GetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}

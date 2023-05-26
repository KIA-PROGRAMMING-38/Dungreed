using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class BossRoomPresenter : MonoBehaviour
{
    private BossBase _boss;
    private BossRoom _room;

    [SerializeField]
    private BossHealthBar _healthBar;

    private bool _isHealthBarFading;
    [SerializeField] private float _healthBarFadeTime = 1f;
    public BossHealthBar HealthBar { get { return _healthBar; } }
    private Vector2 _healthBarStartPosition;
    private Vector2 _healthBarPivotPosition;

    public void Awake()
    {
        HealthBar.gameObject.SetActive(false);
    }

    public void Bind(BossBase boss, BossRoom bossRoom)
    {
        _boss = boss;
        _room = bossRoom;
        _healthBar.SetOwnerHealth(_boss.GetComponent<Health>());

        _healthBarPivotPosition = _healthBar.transform.position;
        _healthBarStartPosition = _healthBarPivotPosition;
        _healthBarStartPosition.y -= _healthBar.GetComponent<RectTransform>().rect.height;
    }

    public void FadeOutHealthBar()
    {
        if (_isHealthBarFading == true) return;

        FadeOutHealthBarTask().Forget();
    }

    public void FadeInHealthBar()
    {
        if (_isHealthBarFading == true) return;
        HealthBar.gameObject.SetActive(true);
        FadeInHealthBarTask().Forget();
    }

    public async UniTaskVoid FadeInHealthBarTask()
    {
        _isHealthBarFading = true;
        _healthBar.transform.position = _healthBarStartPosition;
        Color col = Color.white;
        col.a = 0;
        for (int i = 0; i < _healthBar.Images.Length; ++i)
        {
            _healthBar.Images[i].color = col;
        }
        float t = 0;

        while (t - 0.1f < _healthBarFadeTime)
        {
            t += Time.deltaTime;
            float timeRatio = t / _healthBarFadeTime;
            float alphaRatio = Mathf.Lerp(0f, 1f, timeRatio);
            Vector2 position = Vector2.Lerp(_healthBarStartPosition, _healthBarPivotPosition, timeRatio);
            col.a = alphaRatio;

            for (int i = 0; i < _healthBar.Images.Length; ++i)
            {
                _healthBar.Images[i].color = col;
            }

            _healthBar.transform.position = position;
            await UniTask.Yield();
        }
        _isHealthBarFading = false;
    }

    public async UniTaskVoid FadeOutHealthBarTask()
    {
        _isHealthBarFading = true;
        _healthBar.transform.position = _healthBarStartPosition;
        Color col = Color.white;
        col.a = 0;
        for (int i = 0; i < _healthBar.Images.Length; ++i)
        {
            _healthBar.Images[i].color = col;
        }
        float t = 0;

        while (t - 0.1f < _healthBarFadeTime)
        {
            t += Time.deltaTime;
            float timeRatio = 1 - (t / _healthBarFadeTime);
            float alphaRatio = Mathf.Lerp(0f, 1f, timeRatio);
            Vector2 position = Vector2.Lerp(_healthBarStartPosition, _healthBarPivotPosition, timeRatio);
            col.a = alphaRatio;

            for (int i = 0; i < _healthBar.Images.Length; ++i)
            {
                _healthBar.Images[i].color = col;
            }

            _healthBar.transform.position = position;
            await UniTask.Yield();
        }
        _isHealthBarFading = false;
    }
}

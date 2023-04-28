using System;
using System.Collections;
using UnityEngine;

public class BossRoom : RoomBase
{
    [SerializeField] private string _bossRoomBgm;
    [SerializeField] private InitPosition _startPosition;
    private BossBase    _boss;

    private BossCutScene _cutScene;
    private BossEndCutScene _endScene;

    public event Action OnBossBattleStart;
    public event Action OnBossBattleEnd;


    private bool        _isBossCleared = false;
    private bool        _isBattleStart = false;

    public bool IsBattleStart { get { return _isBattleStart; } }

    public bool IsBossCleared 
    {
        get 
        { 
            return _isBossCleared;  
        } 
        set
        {
            _isBossCleared = value;
            if(IsBossCleared)
            {
                _endScene.ProcessCutScene(null, OnBossBattleEnd);
                UIBinder.Instance.BossRoomPresenter.FadeOutHealthBar();
                OnRoomClear?.Invoke();
            }
        } 
    }

    public override void Initialize()
    {
        base.Initialize();
        if(_boss == null)
        {
            _boss = this.GetComponentAllCheck<BossBase>();
        }

        _cutScene = GetComponent<BossCutScene>();
        _endScene = GetComponent<BossEndCutScene>();
        _boss?.Initialize(this);

        UIBinder.Instance.BindBossRoomPresenter(_boss, this);
    }

    public override void OnRoomEnter()
    {
        SoundManager.Instance.BGMPlay(_bossRoomBgm);
        _player.transform.position = _startPosition.Position;
        GameManager.Instance.CameraManager.SettingCamera(RoomBounds, _startPosition);
        _player.transform.position = _startPosition.Position;
        _player.GetComponent<PlayerController>().StopController();

        UIBinder.Instance.BossRoomPresenter.HealthBar.FadeInImages();
        UIBinder.Instance.BossRoomPresenter.FadeInHealthBar();

        _cutScene.ProcessCutScene(null, BattleStart);
    }

    public void BattleStart()
    {
        _isBattleStart = true;
        _player.GetComponent<PlayerController>().PlayController();
        OnBossBattleStart?.Invoke();
    }

    public override void OnRoomExit()
    {
    }

    public override void OnRoomStay()
    {
        if(_isBattleStart == false)
        {
            return;
        }
    }
}

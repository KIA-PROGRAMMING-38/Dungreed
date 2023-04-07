using UnityEngine;

public class PlayerData : MonoBehaviour
{

    [Header("Move Data")]
    public float MoveSpeed;


    #region Jump
    [Header("Jump Data")]
    [SerializeField] public int JumpForce;
    [SerializeField] public float JumpTime;

    public bool    IsJumping;
    public float   JumpTimeCounter;
    public ParticleSystem JumpParticle { get; private set; }
    #endregion

    #region Dash
    [Header("Dash Data")]
    public bool CanDash = true;
    public int MaxDashCount = 2;
    public int CurrentDashCount= 0;
    public float DashCountInterval = 2f;
    public float DashPower;
    public float DashTime;
    #endregion

}

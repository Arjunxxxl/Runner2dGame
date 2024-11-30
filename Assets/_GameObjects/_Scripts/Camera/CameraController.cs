using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class CameraShakeData
    {
        public float shakeDuration;
        public float shakeStrength;
        public int vibrato;
        public float randomness;
        public bool snapping;
        public bool fadeOut;
        public ShakeRandomnessMode shakeRandomnessMode;
    }
    
    [SerializeField] private Player player;
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform camT;
    [SerializeField] private Camera cam;
    
    [Header("Menu")]
    [SerializeField] private Vector3 camPosMenu;
    
    [Header("FOV")]
    [SerializeField] private float camFOVMenu;
    [SerializeField] private float camFOVGamePlay;
    [SerializeField] private float fovChangeSpeed;
    private float targetFov;
    
    [Header("Follow")] 
    [SerializeField] private Vector3 followOffSet;
    [SerializeField] private float followMinYPos;
    [SerializeField] private float followSpeed;

    [Header("Pivot")]
    [SerializeField] private Vector3 pivotCenterPos;
    [SerializeField] private Vector3 pivotLeftPos;
    [SerializeField] private Vector3 pivotRightPos;
    private Vector3 pivotPos;
    [SerializeField] private float pivotChangeSpeed;

    [Header("Shake Data")] 
    [SerializeField] private Vector3 camOriginalPos;
    [SerializeField] private CameraShakeData damageTakenShakeData;
    [SerializeField] private CameraShakeData enemyKillTakenShakeData;

    [Header("Testing")]
    [SerializeField] private bool testShake_DmgTaken;
    [SerializeField] private bool testShake_EnemyKillTaken;
    
    public static Action ShakeCameraOnDamageTaken;
    public static Action ShakeCameraOnEnemyTaken;

    private void OnEnable()
    {
        ShakeCameraOnDamageTaken += OnShakeCameraOnDamageTaken;
        ShakeCameraOnEnemyTaken += OnShakeCameraOnEnemyKillTaken;
    }

    private void OnDisable()
    {
        ShakeCameraOnDamageTaken -= OnShakeCameraOnDamageTaken;
        ShakeCameraOnEnemyTaken -= OnShakeCameraOnEnemyKillTaken;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    internal void SetUp(Player player)
    {
        this.player = player;
        
        SetUpRig();
        SetUpPivot();
        SetUpFov();

        camOriginalPos = camT.transform.localPosition;
    }

    private void Update()
    {
        UpdateTargetFov();
        UpdateCurrentFov();
        
        if (GameManager.Instance.GetGameState() != GameState.Gameplay)
        {
            return;
        }

        if (testShake_DmgTaken)
        {
            testShake_DmgTaken = false;
            OnShakeCameraOnDamageTaken();
        }
        
        if (testShake_EnemyKillTaken)
        {
            testShake_EnemyKillTaken = false;
            OnShakeCameraOnEnemyKillTaken();
        }
        
        UpdatePivotPos();
        MovePivot();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState() != GameState.Gameplay)
        {
            return;
        }
        
        RigFollow();
    }

    #region Rig

    private void SetUpRig()
    {
        Vector3 destination = camPosMenu;

        if (destination.y <= followMinYPos)
        {
            destination.y = followMinYPos;
        }
        
        transform.position = destination;
    }
    
    private void RigFollow()
    {
        Vector3 destination = player.transform.position + followOffSet;
        
        if (destination.y <= followMinYPos)
        {
            destination.y = followMinYPos;
        }
        
        transform.position = Vector3.Lerp(transform.position, destination, Time.fixedDeltaTime * followSpeed);
    }

    #endregion

    #region Pivot

    private void SetUpPivot()
    {
        pivotPos = pivotCenterPos;
        pivot.transform.localPosition = pivotPos;
    }
    
    private void UpdatePivotPos()
    {
        if (player.playerMovement.VelocityX >= 0.01f)
        {
            pivotPos = pivotRightPos;
        }
        else if (player.playerMovement.VelocityX <= -0.01f)
        {
            pivotPos = pivotLeftPos;
        }
        else
        {
            pivotPos = pivotCenterPos;
        }
    }

    private void MovePivot()
    {
        pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, pivotPos, Time.fixedDeltaTime * pivotChangeSpeed);
    }

    #endregion

    #region FOV

    private void SetUpFov()
    {
        targetFov = camFOVMenu;
        cam.orthographicSize = targetFov;
    }

    private void UpdateTargetFov()
    {
        GameState gameState = GameManager.Instance.GetGameState();
        
        if (gameState == GameState.MainMenu || gameState == GameState.Settings || gameState == GameState.GameComplete)
        {
            targetFov = camFOVMenu;
        }
        else if (gameState == GameState.Gameplay || gameState == GameState.Pause || gameState == GameState.GameOver)
        {
            targetFov = camFOVGamePlay;
        }
    }

    private void UpdateCurrentFov()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetFov,
            1 - Mathf.Pow(0.5f, Time.deltaTime * fovChangeSpeed));
    }

    #endregion
    
    #region Camera Shake

    private void OnShakeCameraOnDamageTaken()
    {
        camT.DOShakePosition(damageTakenShakeData.shakeDuration, damageTakenShakeData.shakeStrength, 
            damageTakenShakeData.vibrato, damageTakenShakeData.randomness, damageTakenShakeData.snapping, 
            damageTakenShakeData.fadeOut, damageTakenShakeData.shakeRandomnessMode)
            .SetEase(Ease.InOutQuad)
            .OnComplete(OnCheckCompleted);
    }
    
    private void OnShakeCameraOnEnemyKillTaken()
    {
        camT.DOShakePosition(enemyKillTakenShakeData.shakeDuration, enemyKillTakenShakeData.shakeStrength, 
                enemyKillTakenShakeData.vibrato, enemyKillTakenShakeData.randomness, enemyKillTakenShakeData.snapping, 
                enemyKillTakenShakeData.fadeOut, enemyKillTakenShakeData.shakeRandomnessMode)
            .SetEase(Ease.InOutQuad)
            .OnComplete(OnCheckCompleted);
    }

    private void OnCheckCompleted()
    {
        camT.transform.localPosition = camOriginalPos;
    }
    
    #endregion
}

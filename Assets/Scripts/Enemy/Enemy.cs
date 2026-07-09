using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Target")] // 플레이어 추적
    [SerializeField] private Transform player;

    [Header("Movement")] // 몬스터 이동
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float rotationSpeed = 720.0f;
    [SerializeField] private float returnStopDistance = 0.1f;

    [Header("Detection")] // 적 발견
    [SerializeField] private float detectRange = 7.0f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float loseRange = 10.0f;

    [Header("Attack")] // 공격 간격 및 데미지
    [SerializeField] private float attackCoolTime = 1.0f;
    [SerializeField] private int attackDamage = 10;

    [Header("Debug")]
    [SerializeField] private string currentStateName;

    private Vector3 originPosition;
    private EnemyStateMachine stateMachine;
    private PlayerHealth playerHealth;
    private float lastAttackTime = -999f;

    public EnemyIdleState IdleState { get; private set; }
    public EnemyTraceState TraceState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyReturnState ReturnState { get; private set; }

    public Transform Player => player;
    public Vector3 OriginPosition => originPosition;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float ReturnStopDistance => returnStopDistance;
    public float DetectRange => detectRange;
    public float AttackRange => attackRange;
    public float LoseRange => loseRange;
    public float AttackCoolTime => attackCoolTime;
    public int AttackDamage => attackDamage;

    private void Awake()
    {
        // 시작 위치 지정
        originPosition = transform.position;

        // 상ㅌ
        stateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, stateMachine);
        TraceState = new EnemyTraceState(this, stateMachine);
        AttackState = new EnemyAttackState(this, stateMachine);
        ReturnState = new EnemyReturnState(this, stateMachine);

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void Start()
    {
        stateMachine.ChangeState(IdleState);
    }

    private void Update()
    {
        stateMachine.Update();
        currentStateName = stateMachine.CurrentState?.GetType().Name;
    }

    // 탐지 범위 체크
    public bool IsPlayerDetected()
    {
        return player != null && GetDistanceToPlayer() <= detectRange;
    }

    // 공격 범위 체크
    public bool IsPlayerInAttackRange()
    {
        return player != null && GetDistanceToPlayer() <= attackRange;
    }

    // 추적 해제 범위 밖인지 체크
    public bool IsPlayerLost()
    {
        return player == null || GetDistanceToPlayer() >= loseRange;
    }

    // 플레이어와 몬스터 사이 거리 반환 
    public float GetDistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Vector3.Distance(transform.position, player.position);
    }

    // 지정한 위치로 이동.
    public void MoveTo(Vector3 targetPosition)
    {
        // y축 이동 방지: 목표 위치의 높이를 현재 몬스터 높이로 고정
        targetPosition.y = transform.position.y;

        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude <= 0.0001f)
            return;

        transform.position = Vector3.MoveTowards(transform.position,targetPosition,moveSpeed * Time.deltaTime);

        RotateTo(direction);
    }

    // 지정한 방향으로 바라보도록 이동
    public void RotateTo(Vector3 direction)
    {
        direction.y = 0.0f;
        if (direction.sqrMagnitude <= 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCoolTime;
    }

    public void Attack()
    {
        if (!CanAttack())
            return;

        lastAttackTime = Time.time;

        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth를 찾을 수 없습니다.");
            return;
        }

        playerHealth.TakeDamage(attackDamage);
        Debug.Log($"적 공격: {attackDamage}");
    }

    // Scene View Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
}

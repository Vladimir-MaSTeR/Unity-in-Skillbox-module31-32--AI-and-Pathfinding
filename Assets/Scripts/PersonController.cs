using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class PersonController : MonoBehaviour {
    
    #region Target Points fields and Radius

    [SerializeField]
    private Transform _startTarget;

    [SerializeField]
    private Vector3 _startTargetRadiusV3 = new Vector3(0f, 0f, 0f);

    [Space(10)]
    [SerializeField]
    private Transform _targetPointDance;

    [SerializeField]
    private Vector3 _targetRadiusDanceV3 = new Vector3(13f, 0.1f, 17f);

    [Space(10)]
    [SerializeField]
    private Transform _targetPointBar;

    [SerializeField]
    private Vector3 _targetRadiusBarV3 = new Vector3(8f, 0.1f, 13f);

    [Space(10)]
    [SerializeField]
    private Transform _targetPointRelaxRum;

    [SerializeField]
    private Vector3 _targetRadiusRelaxRumV3 = new Vector3(20f, 0.1f, 33f);

    #endregion

    #region Timers fields

    [Space(10)]
    [SerializeField]
    private float _danceTime = 20f;

    [SerializeField]
    private float _idleTime = 10f;

    [SerializeField]
    private float _relaxTime = 10;

    #endregion

    #region PRIVATE FIELDS
    private int _id;

    private NavMeshAgent _agent;
    private Animator _animator;

    private bool _bodySearchTime = false;
    private bool _walk = false;

    private float _currentTimer;
    private Vector3 _currentTargetPoint;
    private String _currenTagVolume;
    private int _zoneNumber;
    #endregion

    #region UNITY STANDART METHODS
    private void Awake() {
        _id = GetInstanceID();
        // Debug.Log($"Присвоен идентификатор = {_id}");
    }

    private void Start() {
        StartActionsGame();
    }

    private void Update() {
        RepeatActionsForUpdateMethod();
    }

    private void OnTriggerEnter(Collider other) {
        SetCurrentTagVolumeForTrigger(other);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(_targetPointDance.position, _targetRadiusDanceV3);
        Gizmos.DrawWireCube(_targetPointBar.position, _targetRadiusBarV3);
        Gizmos.DrawWireCube(_targetPointRelaxRum.position, _targetRadiusRelaxRumV3);
    }
    #endregion

    #region CASTOM METHODS
    private void StartActionsGame() {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _currentTargetPoint = GetRandomPointInVector3(_startTarget.transform, _startTargetRadiusV3);
        ;
        _agent.destination = _currentTargetPoint;
        UseAnimations(true, false, false, GameConstants.NO_DANCE_FOR_ANIM_PERSON);
        _walk = true;
    }

    private void RepeatActionsForUpdateMethod() {
        if(_walk) {
            if(_agent.remainingDistance <= _agent.stoppingDistance) {
                _walk = false;
                DefinitionTagVolumeAndAssignTime();
            }
        } else {
            if(_bodySearchTime) {
                UseAnimations(false, false, true, GameConstants.NO_DANCE_FOR_ANIM_PERSON);
            }

            if(_currentTimer <= 0) {
                _zoneNumber = Random.Range(1, 4);
                UseAnimations(true, false, false, GameConstants.NO_DANCE_FOR_ANIM_PERSON);
                ActionsWithZoneNumber(_zoneNumber);
            } else {
                _currentTimer -= Time.deltaTime;
            }
        }
    }

    private void SetCurrentTagVolumeForTrigger(Collider other) {
        if(other.CompareTag(GameConstants.TAG_DANCE_FOR_VOLUME)) {
            _currenTagVolume = GameConstants.TAG_DANCE_FOR_VOLUME;
        }

        if(other.CompareTag(GameConstants.TAG_BAR_FOR_VOLUME)) {
            _currenTagVolume = GameConstants.TAG_BAR_FOR_VOLUME;
        }

        if(other.CompareTag(GameConstants.TAG_RELAX_FOR_VOLUME)) {
            _currenTagVolume = GameConstants.TAG_RELAX_FOR_VOLUME;
        }
    }
    
    private void ActionsWithZoneNumber(int zoneNumber) {
        if(GameConstants.BAR_ZONE_NUMBER_VALUE == zoneNumber) {
            _currentTargetPoint = GetRandomPointInVector3(_targetPointBar, _targetRadiusBarV3);

            _agent.destination = _currentTargetPoint;
            _walk = true;
        }

        if(GameConstants.DANCE_ZONE_NUMBER_VALUE == zoneNumber) {
            _currentTargetPoint = GetRandomPointInVector3(_targetPointDance, _targetRadiusDanceV3);

            _agent.destination = _currentTargetPoint;
            _walk = true;
        }

        if(GameConstants.RELAX_RUM_ZONE_NUMBER_VALUE == zoneNumber) {
            _currentTargetPoint = GetRandomPointInVector3(_targetPointRelaxRum, _targetRadiusRelaxRumV3);

            _agent.destination = _currentTargetPoint;
            _walk = true;
        }
    }

    private void DefinitionTagVolumeAndAssignTime() {
        if(_currenTagVolume.Equals(GameConstants.TAG_DANCE_FOR_VOLUME)) {
            UseAnimations(false, false, false, Random.Range(1, 4));
            _currentTimer = _danceTime;
        }

        if(_currenTagVolume.Equals(GameConstants.TAG_BAR_FOR_VOLUME)) {
            UseAnimations(false, false, false, GameConstants.NO_DANCE_FOR_ANIM_PERSON);
            _currentTimer = _idleTime;
        }

        if(_currenTagVolume.Equals(GameConstants.TAG_RELAX_FOR_VOLUME)) {
            UseAnimations(false, true, false, GameConstants.NO_DANCE_FOR_ANIM_PERSON);
            _currentTimer = _relaxTime;
        }
    }

    private Vector3 GetRandomPointInVector3(Transform targetPoint, Vector3 searchBounds) {
        Vector3 randomPoint = new Vector3(
        targetPoint.position.x + Random.Range(-searchBounds.x / 2f, 
        searchBounds.x / 2f), targetPoint.position.y,
        targetPoint.position.z + Random.Range(-searchBounds.z / 2f, searchBounds.z / 2f));

        return randomPoint;
    }

    private void UseAnimations(bool walk, bool relax, bool search, int danceValue) {
        _animator.SetBool(GameConstants.WALK_PERSON_NAME_ANIM, walk);
        _animator.SetBool(GameConstants.RELAX_PERSON_NAME_ANIM, relax);
        _animator.SetBool(GameConstants.SEARCH_PERSON_NAME_ANIM, search);
        _animator.SetInteger(GameConstants.DANCE_PERSON_NAME_ANIM, danceValue);
    }
    
    #endregion

    #region GETTERS and SETTERS

    public int GetPersonId { get => _id; }
    public bool GetCurrentBodySearchTime { get => _bodySearchTime; set => _bodySearchTime = value; }
    public bool GetWalkPerson { get => _walk; set => _walk = value; }
    public float GetAndSetCurrentTime { get => _currentTimer; set => _currentTimer = value; }

    #endregion
}

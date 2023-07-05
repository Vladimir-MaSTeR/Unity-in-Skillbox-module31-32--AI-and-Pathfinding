using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class PersonController : MonoBehaviour {
    #region Target Points fields and Radius

    [SerializeField]
    private GameObject _startTarget;

    [SerializeField]
    private Transform _targetPointDance;

    [SerializeField]
    private float _targetRadiusDance;

    [SerializeField]
    private Transform _targetPointBar;

    [SerializeField]
    private float _targetRadiusBar;

    // [SerializeField]
    // private Transform _targetPointRelaxRum;
    //
    // [SerializeField]
    // private float _targetRadiusRelaxRum;

    #endregion

    #region Timers fields

    [SerializeField]
    private float _danceTime = 10f;

    [SerializeField]
    private float _idleTime = 5f;

    [SerializeField]
    private float _relaxTime = 7;

    #endregion

    private NavMeshAgent _agent;
    private Animator _animator;

    private bool _bodySearchTime = false;
    private bool _walk = false;

    private float _currentTimer;
    private Vector3 _currentTargetPoint;
    private String _currenTagVolume;
    private int _zoneNumber;

    private void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _currentTargetPoint = _startTarget.transform.position + Random.insideUnitSphere * _targetRadiusDance;
        _agent.destination = _currentTargetPoint;
        _animator.SetBool("walk", true);
        _walk = true;
    }

    private void Update() {
        if(_walk) {
            Debug.Log($"ИДУ ДО ТОЧКИ {_agent.pathEndPosition}");
            if(_agent.remainingDistance <= 0) {
                Debug.Log($"ПУТЬ ЗАВЕРШЕН  В ПОЗИЦИЕ {_agent.pathEndPosition}");
                _walk = false;

                if(_currenTagVolume.Equals("dance")) {
                    _animator.SetBool("walk", false);
                    _animator.SetInteger("dance", Random.Range(1, 4));

                    _currentTimer = _danceTime;
                }

                if(_currenTagVolume.Equals("bar")) {
                    _animator.SetBool("walk", false);
                    _animator.SetBool("relax", false);
                    _animator.SetInteger("dance", 0);

                    _currentTimer = _idleTime;
                }
            }
        } else {
            if(_currentTimer <= 0) {
                _zoneNumber = Random.Range(1, 3);

                _animator.SetBool("walk", true);
                _animator.SetBool("relax", false);
                _animator.SetInteger("dance", 0);

                if(_zoneNumber == 1) {
                    // зона бара
                    _currentTargetPoint = _targetPointBar.position + Random.insideUnitSphere * _targetRadiusBar;
                    _agent.destination = _currentTargetPoint;
                    // _currentTimer = 50f;
                    _walk = true;
                }

                if(_zoneNumber == 2) {
                    // зона танцпола
                    _currentTargetPoint = _targetPointDance.position + Random.insideUnitSphere * _targetRadiusDance;
                    _agent.destination = _currentTargetPoint;
                    // _currentTimer = 50f;
                    _walk = true;
                }

                //пересчитать точку цели и двигаться к ней
            } else {
                _currentTimer -= Time.deltaTime;
            }
        }

        // if(_agent.pathEndPosition.Equals(_targetPointBar.position * _targetRadiusBar)) {
        //     _animator.SetBool("relax", false);
        //     _animator.SetBool("walk", false);
        //     //запускать таймер 
        // }
        //
        // if(_agent.pathEndPosition.Equals(_targetPointRelaxRum.position * _targetRadiusRelaxRum)) {
        //     _animator.SetBool("walk", false);
        //     _animator.SetBool("relax", true);
        //     //запускать таймер 
        // }
        // } else {
        //     _animator.SetBool("walk", true);
        //     // следить куда идем
        // }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("ЗАДЕЛ ТРИГЕР");

        if(other.CompareTag("dance")) {
            _currenTagVolume = "dance";
        }

        if(other.CompareTag("bar")) {
            _currenTagVolume = "bar";
        }

        if(other.CompareTag("relax")) {
            _currenTagVolume = "relax";
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_startTarget.transform.position, _targetRadiusDance);
        Gizmos.DrawWireSphere(_targetPointBar.position, _targetRadiusBar);
    }
}

using UnityEngine;
using UnityEngine.AI;
public class PoliceController : MonoBehaviour {
    [SerializeField]
    private float _searchTime = 7f;

    private Animator _animator;
    private NavMeshAgent _agent;

    private GameObject[] _allPersons;
    private int _randomIndex;
    private float _currentSearchTime;

    private void Awake() {
        _allPersons = GameObject.FindGameObjectsWithTag("Player");
        // Debug.Log($"Найдено {_allPersons.Length} посетителей");
    }

    private void Start() { StartValue(); }

    private void Update() { ActivityPolice(); }

    private void ActivityPolice() {
        if(_agent.remainingDistance <= _agent.stoppingDistance) {
            if(_currentSearchTime >= 0) {
                if(_allPersons[_randomIndex].GetComponent<PersonController>().GetWalkPerson == false) {
                    _animator.SetBool(AnimationConstants.WALK_POLICCE_NAME_ANIM, false);
                    // Debug.Log("Обыскиваю");

                    _allPersons[_randomIndex].GetComponent<PersonController>().GetCurrentBodySearchTime = true;
                    _allPersons[_randomIndex].transform.LookAt(this.gameObject.transform);
                    this.transform.LookAt(_allPersons[_randomIndex].transform);

                    _currentSearchTime -= Time.deltaTime;
                } else {
                    _allPersons[_randomIndex].GetComponent<PersonController>().GetCurrentBodySearchTime = false;
                    // _allPersons[_randomIndex].GetComponent<PersonController>().GetAndSetCurrentTime = 0;

                    _randomIndex = Random.Range(0, _allPersons.Length);
                    _agent.destination = _allPersons[_randomIndex].transform.position;
                    _animator.SetBool(AnimationConstants.WALK_POLICCE_NAME_ANIM, true);
                }
            } else {
                _allPersons[_randomIndex].GetComponent<PersonController>().GetCurrentBodySearchTime = false;
                _allPersons[_randomIndex].GetComponent<PersonController>().GetAndSetCurrentTime = 0;

                _randomIndex = Random.Range(0, _allPersons.Length);
                _agent.destination = _allPersons[_randomIndex].transform.position;

                _animator.SetBool(AnimationConstants.WALK_POLICCE_NAME_ANIM, true);
                _currentSearchTime = _searchTime;
            }
        } else {
            _agent.destination = _allPersons[_randomIndex].transform.position;
        }
    }
    
    private void StartValue() {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _currentSearchTime = _searchTime;

        if(_allPersons.Length > 0) {
            _randomIndex = Random.Range(0, _allPersons.Length);
            _agent.destination = _allPersons[_randomIndex].transform.position;
            _animator.SetBool(AnimationConstants.WALK_POLICCE_NAME_ANIM, true);
        }
    }
}

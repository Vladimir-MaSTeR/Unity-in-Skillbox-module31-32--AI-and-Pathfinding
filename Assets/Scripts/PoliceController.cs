using UnityEngine;
using UnityEngine.AI;
public class PoliceController : MonoBehaviour {
    [SerializeField]
    private float _searchTime = 7f;

    #region PRIVATE FIELDS

    private Animator _animator;
    private NavMeshAgent _agent;

    private GameObject[] _allPersons;
    private int _randomIndex;
    private float _currentSearchTime;

    #endregion

    #region UNITY STANDART METHODS

    private void Awake() {
        _allPersons = GameObject.FindGameObjectsWithTag("Player");
        // Debug.Log($"Найдено {_allPersons.Length} посетителей");
    }

    private void Start() { StartValue(); }

    private void Update() { ActivityPolice(); }

    #endregion

    #region CASTOM METHODS

    private void ActivityPolice() {
        if(_agent.remainingDistance <= _agent.stoppingDistance) {
            if(_currentSearchTime >= 0) {
                if(_allPersons[_randomIndex].GetComponent<PersonController>().GetWalkPerson == false) {
                    SetAnimations(false, true);
                    // Debug.Log("Обыскиваю");

                    _allPersons[_randomIndex].GetComponent<PersonController>().GetCurrentBodySearchTime = true;
                    _allPersons[_randomIndex].transform.LookAt(this.gameObject.transform);
                    this.transform.LookAt(_allPersons[_randomIndex].transform);

                    _currentSearchTime -= Time.deltaTime;
                } else {
                    _allPersons[_randomIndex].GetComponent<PersonController>().GetCurrentBodySearchTime = false;

                    _randomIndex = Random.Range(0, _allPersons.Length);
                    _agent.destination = _allPersons[_randomIndex].transform.position;
                    SetAnimations(true, false);
                }
            } else {
                _allPersons[_randomIndex].GetComponent<PersonController>().GetCurrentBodySearchTime = false;
                _allPersons[_randomIndex].GetComponent<PersonController>().GetAndSetCurrentTime = 0;

                _randomIndex = Random.Range(0, _allPersons.Length);
                _agent.destination = _allPersons[_randomIndex].transform.position;

                SetAnimations(true, false);
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
            SetAnimations(true, false);
        }
    }

    private void SetAnimations(bool walk, bool search) {
        _animator.SetBool(GameConstants.WALK_POLICE_NAME_ANIM, walk);
        _animator.SetBool(GameConstants.SEARCH_POLICE_NAME_ANIM, search);
    }

    #endregion
}

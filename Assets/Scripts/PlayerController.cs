using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace {
    public class PlayerController : MonoBehaviour {
        [SerializeField]
        private GameObject _target;

        private NavMeshAgent _agent;

        private void Start() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.destination = _target.transform.position;
        }
    }
}

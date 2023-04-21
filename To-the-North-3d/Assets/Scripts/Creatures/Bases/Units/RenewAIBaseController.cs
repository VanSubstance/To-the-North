using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures.Bases.Units
{
    public class RenewAIBaseController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 testTargetPos;
        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            agent.SetDestination(testTargetPos);
        }
    }
}

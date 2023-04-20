using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    internal class TrajectoryManager : MonoBehaviour
    {
        [SerializeField]
        private Transform projectilePrefab;
        private List<TrajectoryController> trajectories;

        private static TrajectoryManager _instance;
        public static TrajectoryManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(TrajectoryManager)) as TrajectoryManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            if (transform.childCount == 0)
            {
                trajectories = new List<TrajectoryController>();
                for (int i = 0; i < 100; i++)
                {
                    trajectories.Add(Instantiate(projectilePrefab, transform).GetComponent<TrajectoryController>());
                }
            }
        }

        public TrajectoryController GetNewTrajectory()
        {
            for (int i = 0; i < trajectories.Count; i++)
            {
                if (!trajectories[i].IsPossessed && !trajectories[i].gameObject.activeSelf)
                {
                    return trajectories[i];
                }
            }
            return null;
        }
    }
}

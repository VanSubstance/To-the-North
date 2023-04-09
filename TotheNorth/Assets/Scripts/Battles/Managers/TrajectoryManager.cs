using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    internal class TrajectoryManager : MonoBehaviour
    {
        [SerializeField]
        private Transform projectilePrefab;
        private List<TrajectoryController> trajectories;

        // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
        private static TrajectoryManager _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static TrajectoryManager Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
            // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
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

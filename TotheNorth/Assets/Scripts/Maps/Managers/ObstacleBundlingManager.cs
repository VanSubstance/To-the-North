using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Maps.Controllers;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Maps.Managers
{
    internal class ObstacleBundlingManager : MonoBehaviour
    {
        public Transform obstacleParent;
        [SerializeField]
        private Transform compositeObjectPrefab;

        private Queue<ObstacleController> obsQueue = new Queue<ObstacleController>();
        private void Awake()
        {
            Transform compositeObstacleParent = new GameObject("Obstacles").transform;
            compositeObstacleParent.SetParent(null);
            compositeObstacleParent.position = Vector3.zero;
            compositeObstacleParent.localScale = Vector3.one;

            while (obstacleParent.childCount > 0)
            {
                Transform compositeObstacle = Instantiate(compositeObjectPrefab, compositeObstacleParent);
                compositeObstacle.position = Vector3.zero;
                compositeObstacle.localScale = Vector3.one;
                // 1. 타겟 설정
                ObstacleController curObs = obstacleParent.GetChild(0).GetComponent<ObstacleController>();
                DetectAndHandOver(curObs, compositeObstacle);
                compositeObstacle.AddComponent<CompositeCollider2D>();
                compositeObstacle.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;
            }
            obsQueue.Clear();
            //Destroy(obstacleParent.gameObject);
        }

        private void DetectAndHandOver(ObstacleController curObs, Transform compositeObstacle)
        {
            Collider2D[] temp = curObs.GetContactedColliders();
            // 2. 주변 장애물 식별
            foreach (Collider2D collider in temp)
            {
                if (collider == null) break;
                obsQueue.Enqueue(collider.transform.parent.GetComponent<ObstacleController>());
            }
            // 3. 타겟 먼저 넘기기
            curObs.Composite(compositeObstacle);
            // 4. 식별된 애들 2번부터 반복
            while (obsQueue.TryDequeue(out curObs))
            {
                if (curObs.CurrentLayer == 14) continue;
                DetectAndHandOver(curObs, compositeObstacle);
            }
        }
    }
}

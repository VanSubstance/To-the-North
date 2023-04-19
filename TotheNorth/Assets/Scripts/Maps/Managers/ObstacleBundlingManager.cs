using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Maps.Managers
{
    internal class ObstacleBundlingManager : MonoBehaviour
    {
        public Transform obstacleParent;
        [SerializeField]
        private Transform compositeObjectPrefab;

        private Transform compositeObstacleParent;

        private Queue<ObstacleController> obsQueue = new Queue<ObstacleController>();
        private void Awake()
        {
            compositeObstacleParent = new GameObject("Obstacles").transform;
            compositeObstacleParent.SetParent(null);
            compositeObstacleParent.position = Vector3.zero;
            compositeObstacleParent.localScale = Vector3.one;
            CompositeObstacles(obstacleParent);
            obsQueue.Clear();
            //Destroy(obstacleParent.gameObject);
        }

        /// <summary>
        /// 장애물 합치기
        /// </summary>
        /// <param name="target"></param>
        private void CompositeObstacles(Transform target)
        {
            ObstacleController curOb;
            if ((curOb = target.GetComponent<ObstacleController>()) != null)
            {
                // 단일 장애물
                // 이거랑 연결되어있는 장애물 전부 통합 및 뱉어내기
                Transform compositeObstacle = Instantiate(compositeObjectPrefab, compositeObstacleParent);
                compositeObstacle.position = Vector3.zero;
                compositeObstacle.localScale = Vector3.one;
                CompositeSingle(curOb, compositeObstacle);
                compositeObstacle.AddComponent<CompositeCollider2D>();
                compositeObstacle.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;
            }
            else
            {
                // 복합 장애물
                // 자식들 중에 장애물 찾기
                for (int i = 0; i < target.childCount; i++)
                {
                    CompositeObstacles(target.GetChild(i));
                }
            }
        }

        private void CompositeSingle(ObstacleController startOb, Transform compositeObstacle)
        {
            Collider2D[] temp = startOb.GetContactedColliders();
            // 2. 주변 장애물 식별
            foreach (Collider2D collider in temp)
            {
                if (collider == null) break;
                obsQueue.Enqueue(collider.transform.parent.GetComponent<ObstacleController>());
            }
            // 3. 타겟 먼저 넘기기
            startOb.Composite(compositeObstacle);
            // 4. 식별된 애들 2번부터 반복
            while (obsQueue.TryDequeue(out startOb))
            {
                if (startOb.CurrentLayer == 14) continue;
                CompositeSingle(startOb, compositeObstacle);
            }
        }
    }
}

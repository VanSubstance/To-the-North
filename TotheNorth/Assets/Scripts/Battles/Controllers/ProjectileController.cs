using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Commons.Functions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Battles
{
    internal class ProjectileController : MonoBehaviour
    {
        [SerializeField]
        private ProjectileInfo info;

        private float prevDis = float.MaxValue;
        //private Vector3 targetPos;
        private LineRenderer line;
        private List<Vector3> positions;

        public Vector3 startPos, targetPos;

        public void Fire(ProjectileInfo _info, Vector3 startPos, Vector3 targetDir)
        {
            targetDir.z = 0f;

            this.startPos = startPos;

            info = ProjectileInfo.GetClone(_info);
            transform.position = startPos + LocalPostionToWorld(info.StartPos, targetDir);
            targetPos = LocalPostionToWorld(info.EndPos - info.StartPos, targetDir);
            prevDis = float.MaxValue;
            gameObject.SetActive(true);
            GetComponent<Rigidbody2D>().velocity = targetPos.normalized * info.Spd;
            targetPos *= (info.EndPos - info.StartPos).magnitude;
            targetPos += transform.position;
        }
        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            positions = new List<Vector3>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (
                positions.Count > 0 &&
                Vector3.Distance(positions[positions.Count - 1], transform.position) < .1f)
            {
                return;
            }
            if (transform.position == Vector3.zero) return;
            if (Vector3.Distance(transform.position, targetPos) >= prevDis)
            {
                gameObject.SetActive(false);
            }
            else prevDis = Vector3.Distance(transform.position, targetPos);
            positions.Add(transform.position);
            if (positions.Count > 10)
            {
                positions.RemoveAt(0);
            }
            DrawTail();
        }

        private void DrawTail()
        {
            line.positionCount = positions.Count;
            line.SetPositions(positions.ToArray());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.layer)
            {
                case 8:
                case 13:
                case 15:
                    Debug.Log("크리쳐와 충돌");
                    //collision.transform.GetComponent<ICreatureBattle>().Hit(info);
                    break;
                case 14:
                    Debug.Log("장애물과 충돌");
                    break;
            }
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            transform.position = Vector3.zero;
            line.positionCount = 0;
            positions = new List<Vector3>();
        }

        private Vector3 LocalPostionToWorld(Vector3 targetPos, Vector3 targetDir)
        {
            return 2 * CalculationFunctions.GetRotatedVector2(
                targetPos.magnitude * targetDir.normalized,
                CalculationFunctions.AngleFromDir(
                    targetPos.normalized
                    )
                );
        }
    }
}

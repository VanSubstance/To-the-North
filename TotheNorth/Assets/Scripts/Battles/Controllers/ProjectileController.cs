using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Battles
{
    internal class ProjectileController : MonoBehaviour
    {
        private ProjectileInfo info;
        private LineRenderer line;
        private List<Vector3> positions;
        public void Fire(ProjectileInfo _info, Vector3 startPos, Vector3 _directionTarget)
        {
            info = ProjectileInfo.GetClone(_info);
            transform.position = startPos;
            GetComponent<Rigidbody2D>().velocity = _directionTarget;
            gameObject.SetActive(true);
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
        }
    }
}

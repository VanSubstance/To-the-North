using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Maps.Controllers
{
    internal class TrackBaseController: MonoBehaviour
    {
        private void Awake()
        {
            Destroy(GetComponent<SpriteRenderer>());
        }
    }
}

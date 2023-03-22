using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Creatures.Abstracts
{
    internal class AAIConductionController : MonoBehaviour
    {
        protected AAIBaseController baseController;
        protected void Awake()
        {
            baseController = GetComponent<AAIBaseController>();
        }
    }
}

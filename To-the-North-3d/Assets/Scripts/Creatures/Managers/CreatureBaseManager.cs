using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    internal class CreatureBaseManager : MonoBehaviour
    {
        [SerializeField]
        private Transform hpBarPrefab, panelFull;

        private Transform[] hpBars;
        private Transform parentForHpBars;

        private void Awake()
        {
            Transform temp = GameObject.Find("UI").transform;
            if (parentForHpBars = temp.Find("Panel for Hp of Creatures"))
            {
                // 있음
            }
            else
            {
                // 없음
                parentForHpBars = Instantiate(panelFull, temp);
                parentForHpBars.name = "Panel for Hp of Creatures";
            }
            Transform creatureParent = transform.Find("Creatures");
            hpBars = new Transform[creatureParent.childCount];
            for (int i = 0; i < creatureParent.childCount; i++)
            {
                GenerateHpBarForCreature(creatureParent.GetChild(i).GetComponent<AIBaseController>());
            }
        }

        private void GenerateHpBarForCreature(AIBaseController _base)
        {
            Transform hpBar = Instantiate(hpBarPrefab, parentForHpBars);
            _base.SetHpBar(hpBar);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival.Cheat
{
    public class CheatMiningMouse : MonoBehaviour
    {
        [SerializeField] int miningValue = 1;
        [SerializeField] float miningRadius = 1f;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), miningRadius);
                foreach (var hit in hits)
                {
                    IBreakable breakable = hit.GetComponent<IBreakable>();
                    if (breakable != null)
                    {
                        breakable.Mine(miningValue);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), miningRadius);
        }
    }
}

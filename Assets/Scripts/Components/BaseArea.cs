using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public abstract class BaseArea: MonoBehaviour
    {
        public enum AreaShape { Box, Circle }

        [SerializeField] protected AreaShape shape = AreaShape.Box;
        [SerializeField] protected Vector2 size = new Vector2(10f, 10f);
        [SerializeField] protected float radius = 5f;

        [SerializeField] private Color gizmoColor = Color.gray;
        public AreaShape Shape => shape;
        protected Vector3 halfSize = new Vector3();

        protected virtual void Awake()
        {
            halfSize = new Vector3(size.x / 2, size.y / 2, 0f);

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            switch (shape)
            {
                case AreaShape.Box:
                    Gizmos.DrawWireCube(transform.position, size);
                    break;
                case AreaShape.Circle:
                    Gizmos.DrawWireSphere(transform.position, radius);
                    break;
            }
        }

        public bool Contains(Vector2 point)
        {
            switch (shape)
            {
                case AreaShape.Box:
                    return Tools.InBetween(point.x, transform.position.x - halfSize.x, transform.position.x + halfSize.x) &&
                           Tools.InBetween(point.y, transform.position.y - halfSize.y, transform.position.y + halfSize.y);
                case AreaShape.Circle:
                    return Vector3.Distance(transform.position, point) <=radius;
                default:
                    return false;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public static class Tools
    {
        public static T FindTopLevelEntity<T>(Transform transform) where T : class
        {
            while (transform != null)
            {
                T entity = transform.GetComponent<T>();
                if (entity != null)
                {
                    return entity;
                }
                transform = transform.parent; // Move up the hierarchy
            }

            return null; // No IEntity found
        }

        public static bool InBetween(float value, float min, float max)
        {
            return (value>=min && value<= max);
        }
        public static void CopyFields(object source, object destination)
        {
            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            foreach (var field in sourceType.GetFields())
            {
                var destField = destinationType.GetField(field.Name);
                if (destField != null)
                {
                    destField.SetValue(destination, field.GetValue(source));
                }
            }
        }
    }
}

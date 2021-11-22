using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace IBL.BO
{
    public static class DeepCopy
    {
        public static void CopyPropertiesTo1<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propFrom in from.GetType().GetProperties())
            {
                foreach (PropertyInfo propTo in to.GetType().GetProperties())
                {
                    if (propTo == null)
                    {
                        continue;
                    }
                    object value = propFrom.GetValue(from, null);
                    if ((value is ValueType || value is string) &&
                        propTo.Name == propFrom.Name &&
                        propTo.PropertyType == propFrom.PropertyType
                        && !(propFrom is IEnumerable))
                    {
                        propTo.SetValue(to, value);
                        break;
                    }
                    else
                    {
                        CopyPropertiesTo1(propTo, to);
                    }
                }
            }
        }

        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                {
                    continue;
                }
                object value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                {
                    propTo.SetValue(to, value);
                }
                else if (!(value is IEnumerable))
                {
                    object target = propTo.GetValue(to, null);
                    value.CopyPropertiesTo(target);
                }
            }
        }

       
        public static void CopyPropertiesToIEnumerable<T, S>(this IEnumerable<S> from, List<T> to)
            where T : new()
        {
            foreach (S s in from)
            {
                T t = new T();
                s.CopyPropertiesTo(t);
                to.Add(t);
            }
        }
    }
}

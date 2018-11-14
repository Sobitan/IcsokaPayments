using System;
using System.Data.Entity.Core.Objects;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Data.Extensions
{
    public static class Extensions
    {
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static Type GetUnproxiedEntityType(this BaseEntity entity)
        {
            var userType = ObjectContext.GetObjectType(entity.GetType());
            return userType;
        }
    }
}

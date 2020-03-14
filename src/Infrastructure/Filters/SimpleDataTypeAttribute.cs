using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenMail.Infrastructure.Filters
{
    /// <summary>
    /// Use int entities like ...    
    /// 
    /// typeof(T)
    //  .GetProperties()
    //  .Where(z => z.GetCustomAttributes(typeof(SimplePropertyAttribute), true).Any()
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]

    public class SimpleDataTypeAttribute : Attribute
    {
        public SimpleDataTypeAttribute()
        {
        }
    }
}

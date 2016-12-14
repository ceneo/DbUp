using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbUp.Helpers
{
    /// <summary>
    /// Extensions used in retrieve datas from db
    /// </summary>
    public static class DbExtensions
    {
        /// <summary>
        /// Gets value of field of specified type
        /// </summary>
        /// <typeparam name="T">Returne value type</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Get<T>(this object source)
        {
            if (source == DBNull.Value)
                return default(T);

            return (T)source;

        }
    }
}

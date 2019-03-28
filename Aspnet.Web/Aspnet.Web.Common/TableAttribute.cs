using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Web.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public string Name { get; private set; }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }

    public static class TableExtensions
    {
        private static readonly ConcurrentDictionary<string, string> _tableNameDictionary;

        static TableExtensions()
        {
            _tableNameDictionary = new ConcurrentDictionary<string, string>();
        }

        public static string GetTableName(this Type type)
        {
            if (_tableNameDictionary.ContainsKey(type.FullName))
            {
                return _tableNameDictionary[type.FullName];
            }
            else
            {
                var tableName = (type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute)?.Name ?? type.Name;
                _tableNameDictionary.TryAdd(type.FullName, tableName);
                return tableName;
            }
        }
    }
}

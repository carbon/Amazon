using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Amazon.Elb
{
    internal sealed class InstanceModel
    {
        private static readonly ConcurrentDictionary<Type, InstanceModel> models = new ConcurrentDictionary<Type, InstanceModel>();

        private readonly IReadOnlyList<InstanceModelMember> members;

        public InstanceModel(IReadOnlyList<InstanceModelMember> members)
        {
            this.members = members;
        }

        public IReadOnlyList<InstanceModelMember> Members => members;

        public static InstanceModel Get(Type type)
        {
            if (models.TryGetValue(type, out var result))
            {
                return result;
            }

            var properties = type.GetProperties();

            var members = new List<InstanceModelMember>(properties.Length);

            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<IgnoreDataMemberAttribute>() != null)
                {
                    continue;
                }

                var dataMember = property.GetCustomAttribute<DataMemberAttribute>();

                members.Add(new InstanceModelMember(dataMember?.Name ?? property.Name, property));
            }

            result = new InstanceModel(members);

            models.TryAdd(type, result);

            return result;
        }
    }

    public sealed class InstanceModelMember
    {
        public InstanceModelMember(string name, PropertyInfo property)
        {
            Name = name;
            Property = property;
        }

        public string Name { get; set; }

        public PropertyInfo Property { get; }

        public object GetValue(object instance)
        {
            return Property.GetValue(instance);
        }
    }
}

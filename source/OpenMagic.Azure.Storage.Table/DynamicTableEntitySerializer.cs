using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace OpenMagic.Azure.Storage.Table
{
    public class DynamicTableEntitySerializer<TEntity> : IDynamicTableEntitySerializer<TEntity>
    {
        // todo: use self compiled code for faster operation

        private readonly Func<string, string, TEntity> _entityFactory;
        private readonly Dictionary<string, Func<TEntity, EntityProperty>> _entityPropertyFactories;
        private readonly Func<TEntity, string> _partitionKeyFactory;
        private readonly Dictionary<string, Action<TEntity, EntityProperty>> _propertyWriters;
        private readonly Func<TEntity, string> _rowKeyFactory;

        public DynamicTableEntitySerializer(Func<string, string, TEntity> entityFactory, Func<TEntity, string> partitionKeyFactory, Func<TEntity, string> rowKeyFactory, IEnumerable<string> ignoreProperties)
        {
            _entityFactory = entityFactory;
            _partitionKeyFactory = partitionKeyFactory;
            _rowKeyFactory = rowKeyFactory;

            var properties = typeof(TEntity).GetProperties()
                .Where(p => !ignoreProperties.Contains(p.Name))
                .ToArray();

            _propertyWriters = properties.ToDictionary(p => p.Name, GetPropertyWriter);
            _entityPropertyFactories = properties.ToDictionary(p => p.Name, GetEntityPropertyFactory);
        }

        public TEntity Deserialize(DynamicTableEntity row)
        {
            var entity = _entityFactory(row.PartitionKey, row.RowKey);

            foreach (var column in row.Properties)
            {
                Action<TEntity, EntityProperty> propertyWriter;

                if (_propertyWriters.TryGetValue(column.Key, out propertyWriter))
                {
                    propertyWriter(entity, column.Value);
                }
            }

            return entity;
        }

        public DynamicTableEntity Serialize(TEntity entity)
        {
            return new DynamicTableEntity
            {
                PartitionKey = _partitionKeyFactory(entity),
                RowKey = _rowKeyFactory(entity),
                Properties = _entityPropertyFactories.ToDictionary(e => e.Key, e => e.Value(entity))
            };
        }

        private static Func<TEntity, EntityProperty> GetEntityPropertyFactory(PropertyInfo propertyInfo)
        {
            if ((propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string)) || IsList(propertyInfo.PropertyType))
            {
                return entity => EntityProperty.GeneratePropertyForString(JsonConvert.SerializeObject(propertyInfo.GetValue(entity)));
            }
            return entity => EntityProperty.CreateEntityPropertyFromObject(propertyInfo.GetValue(entity));
        }

        private static Action<TEntity, EntityProperty> GetPropertyWriter(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsEnum)
            {
                return (entity, column) => propertyInfo.SetValue(entity, Enum.Parse(propertyInfo.PropertyType, column.StringValue));
            }
            if ((propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string)) || IsList(propertyInfo.PropertyType))
            {
                return (entity, column) => propertyInfo.SetValue(entity, JsonConvert.DeserializeObject(column.StringValue, propertyInfo.PropertyType));
            }
            return (entity, column) => propertyInfo.SetValue(entity, column.PropertyAsObject);
        }

        private static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
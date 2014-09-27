﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Pineapple.Model;

namespace Pineapple.Data.Sqlite
{
    public class MappingData : IMappingData
    {
        public Mapping<TKey, TValue> SaveMapping<TKey, TValue>(Mapping<TKey, TValue> mapping)
        {
            using (var cnn = SqLiteBaseRepository.DbConnection())
            {
                string insertFormat = "Insert into {0} ({1},{2}) values(@Key,@Value)";
                string updateFormat = "update {0} set {1}=@Key,{2}=@Value where MappingId=@MappingId";

                foreach (var item in mapping.Items)
                {
                    if (item.MappingId <= 0)
                    {
                        item.MappingId = cnn.Query<int>(string.Format(insertFormat, mapping.MappingName, mapping.KeyName, mapping.ValueName)
                            , item).FirstOrDefault();
                    }
                    else
                    {
                        cnn.Execute(string.Format(updateFormat, mapping.MappingName, mapping.KeyName, mapping.ValueName), item);
                    }
                }
                return mapping;
            }
        }

        public bool DeleteMapping<TKey, TValue>(Mapping<TKey, TValue> mapping)
        {
            using (var cnn = SqLiteBaseRepository.DbConnection())
            {
                string delteFormat = "delete from {0} where {1}=@Key and {2}=@Value";
                int total = mapping.Items.Sum(item => cnn.Execute(string.Format(delteFormat, mapping.MappingName, mapping.KeyName, mapping.ValueName), item));
                return total > 0;
            }
        }

        public bool DeleteMappingByKey<TKey, TValue>(Mapping<TKey, TValue> mapping, TKey key)
        {
            using (var cnn = SqLiteBaseRepository.DbConnection())
            {
                string deleteFromat = "delete from {0} where {1}=@Key";
                int total = mapping.Items.Sum(item => cnn.Execute(string.Format(deleteFromat, mapping.MappingName, mapping.KeyName)));
                return total > 0;
            }
        }

        public bool DeleteMappingByValue<TKey, TValue>(Mapping<TKey, TValue> mapping, TValue value)
        {
            using (var cnn = SqLiteBaseRepository.DbConnection())
            {
                string deleteFromat = "delete from {0} where {1}=@Value";
                int total = mapping.Items.Sum(item => cnn.Execute(string.Format(deleteFromat, mapping.MappingName, mapping.ValueName)));
                return total > 0;
            }
        }

        public Mapping<TKey, TValue> GetMappingByKey<TKey, TValue>(Mapping<TKey, TValue> mapping, TKey key)
        {
            using (var cnn = SqLiteBaseRepository.DbReadOnlyConnection())
            {
                var items = cnn.Query<MappingItem<TKey, TValue>>(string.Format("select MappingId,{0} as Key,{1} as Value from {2} where {0}=@Key"
                    , mapping.KeyName, mapping.ValueName, mapping.MappingName), new { Key = key });
                mapping.AddItems(items);
                return mapping;
            }
        }

        public Mapping<TKey, TValue> GetMappingByValue<TKey, TValue>(Mapping<TKey, TValue> mapping, TValue value)
        {
            using (var cnn = SqLiteBaseRepository.DbReadOnlyConnection())
            {
                var items = cnn.Query<MappingItem<TKey, TValue>>(string.Format("select MappingId,{0} as Key,{1} as Value from {2} where {1}=@Value"
                    , mapping.KeyName, mapping.ValueName, mapping.MappingName), new { Value = value });
                mapping.AddItems(items);
                return mapping;
            }
        }
    }
}
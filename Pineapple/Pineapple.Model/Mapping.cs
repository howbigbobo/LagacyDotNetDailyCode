﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Pineapple.Model
{
    public class MappingItem<TKey, TValue>
    {
        public int MappingId { get; set; }
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public MappingItem()
        {

        }
        public MappingItem(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    public class Mapping<TKey, TValue>
    {
        public string MappingName { get; protected set; }
        public string KeyName { get; protected set; }
        public string ValueName { get; protected set; }

        public Mapping() { }
        public Mapping(string mappingName, string keyName, string valueName)
        {
            MappingName = mappingName;
            KeyName = keyName;
            ValueName = valueName;
        }

        private List<MappingItem<TKey, TValue>> _items;
        private HashSet<string> _existKeyValue;

        private HashSet<string> ExistKeyValue
        {
            get { return (_existKeyValue = _existKeyValue ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase)); }
        }

        public List<MappingItem<TKey, TValue>> Items
        {
            get { return (_items = _items ?? new List<MappingItem<TKey, TValue>>()); ; }
        }

        public List<TKey> Keys
        {
            get { return Items.ConvertAll(m => m.Key); }
        }

        public List<TValue> Values
        {
            get { return Items.ConvertAll(m => m.Value); }
        }

        public void AddItem(MappingItem<TKey, TValue> item)
        {
            if (item != null)
            {
                string code = string.Format("{0}_{1}", item.Key, item.Value);
                if (!ExistKeyValue.Contains(code))
                {
                    Items.Add(item);
                    _existKeyValue.Add(code);
                }
            }
        }

        public void AddItems(IEnumerable<MappingItem<TKey, TValue>> items)
        {
            if (items != null)
            {
                foreach (var item in items) AddItem(item);
            }
        }

        public List<MappingItem<TKey, TValue>> FindItemsByKey(TKey key)
        {
            return _items.FindAll(item => item.Key.Equals(key));
        }

        public List<MappingItem<TKey, TValue>> FindItemsByValue(TValue value)
        {
            return _items.FindAll(item => item.Value.Equals(value));
        }

        public MappingItem<TKey, TValue> GetItemByKey(TKey key)
        {
            return _items.Find(item => item.Key.Equals(key));
        }

        public MappingItem<TKey, TValue> GetItemByValue(TValue value)
        {
            return _items.Find(item => item.Value.Equals(value));
        }
    }
}

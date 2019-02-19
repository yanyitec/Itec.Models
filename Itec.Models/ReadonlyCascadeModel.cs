﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Itec.Models
{
    public class ReadonlyCascadeModel:DynamicObject,IReadonlyModel
    {
        public ReadonlyCascadeModel(IReadonlyModel current = null,IReadonlyModel super = null)
        {
            this.Super = super;
            this.Current = current;
        }

        public IReadonlyModel Super { get; private set; }

        public IReadonlyModel Current { get; private set; }

        public string this[string key]
        {
            get { return this.GetString(key); }
        }



        public string GetString(string key = null)
        {
            var value = this.Current.GetString(key);
            if (value == null && this.Super!=null) value = this.Super.GetString(key);
            return value;
        }



        public object GetRaw(string key)
        {
            var value = this.Current.GetRaw(key);
            if (value == null && this.Super != null) value = this.Super.GetRaw(key);
            return value;
        }





        public T Get<T>(string key = null)
        {
            var value = this.Get(typeof(T),key);
            return (T)value;
        }




        public object Get(Type type, string key = null)
        {
            var value = this.Current.Get(type,key);
            if (value== null && this.Super != null) value = this.Super.Get(type,key);
            return value;
        }

        public string ToJSON()
        {
            return this.Current.ToJSON();
        }

        public IList<string> GetMemberNames()
        {
            var names = this.Current.GetMemberNames();
            if (this.Super != null) {
                var superNames = this.Super.GetMemberNames();
                foreach (var name in superNames) {
                    if (names.Contains(name)) names.Add(name);
                }
            }
            return names;
        }

        #region dynamic
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.GetMemberNames();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.Get(binder.ReturnType, binder.Name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var key = indexes[0];

            result = this.Get(binder.ReturnType, key == null ? null : key.ToString());
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return false;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = this.Get(binder.ReturnType);
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            throw new InvalidOperationException("不能当作函数使用");
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            throw new InvalidOperationException("成员不能当作函数使用，成员不能被调用");
        }



        #endregion
    }
}

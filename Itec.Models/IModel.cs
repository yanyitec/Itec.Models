﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Models
{
    /// <summary>
    /// 数据模型
    /// </summary>
    public interface IModel:IReadonlyModel
    {
        /// <summary>
        /// 是否有变更，以便后续确定是否要存储
        /// </summary>
        bool HasChanges { get; }
        /// <summary>
        /// 获取某个特定字段的字符串值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        new string this[string key] { get;set; }

        /// <summary>
        /// 设置特定字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>

        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IModel Set<T>(T value,string key=null);

        IModel Set(Type objectType, object value, string key = null);
        /// <summary>
        /// 用数据源的格式去设置特定字段
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IModel SetRaw( object value, string key=null);
        /// <summary>
        /// 移除某个字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IModel Remove(string key);
    }
}

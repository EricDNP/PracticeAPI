using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PracticeAPI.Models
{
    public interface iEntity
    {
        Guid Id { get; set; }
    }

    public static class EntityConverter
    {
        public static T ConvertEntity<T>(iEntity e) where T : iEntity, new()
        {
            T entity = new T();
            var temp = JsonConvert.SerializeObject(e);
            entity = JsonConvert.DeserializeObject<T>(temp);
            return entity;
        }

        public static ICollection<T> ConvertCollection<T>(object e) where T : iEntity, new()
        {
            ICollection<T> group = new List<T>();
            var temp = JsonConvert.SerializeObject(e);
            group = JsonConvert.DeserializeObject<ICollection<T>>(temp);
            return group;
        }
    }
}
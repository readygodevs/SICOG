using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoreriaVS12.Utils
{
    public class EntityFactory
    {
        public static T getEntity<T>(Object model, Object entity)
        {
            var modelProperties = entity.GetType().GetProperties();
            foreach (var prop in model.GetType().GetProperties())
            {
                var thisProp = modelProperties.FirstOrDefault(n => n.Name == prop.Name && n.PropertyType == prop.PropertyType);
                if (thisProp != null)
                {
                    var value = prop.GetValue(model, null);
                    thisProp.SetValue(entity, value, null);
                }
            }
            return (T)entity;
        }

        private TEntity getEntity<TEntity, TModel>(TEntity entity, TModel model)
            where TEntity : System.Data.Objects.DataClasses.EntityObject, new()
            where TModel : class, new()
        {
            var modelProperties = entity.GetType().GetProperties();
            foreach (var prop in model.GetType().GetProperties())
            {
                var thisProp = modelProperties.FirstOrDefault(n => n.Name == prop.Name);
                if (thisProp != null)
                {
                    var value = prop.GetValue(model, null);
                    thisProp.SetValue(entity, value, null);
                }
            }
            return (TEntity)entity;
        }
    }
}
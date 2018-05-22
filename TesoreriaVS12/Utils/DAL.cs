using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoreriaVS12.Utils
{
    public class DAL
    {
        public T getModel<T>(Object entity, Object model)
        {
            var modelProperties = model.GetType().GetProperties();
            foreach (var prop in entity.GetType().GetProperties())
            {
                var thisProp = modelProperties.FirstOrDefault(n => n.Name == prop.Name);
                if (thisProp != null)
                {
                    var value = prop.GetValue(entity, null);
                    thisProp.SetValue(model, value, null);
                }
            }
            return (T)model;
        }

        public T getEntity<T>(Object model, Object entity)
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
            return (T)entity;
        }
    }
}
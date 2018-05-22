using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    interface IRepository<TEntity>
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null);
        TEntity GetByID(Expression<Func<TEntity, bool>> filter = null);
        void Insert(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> filter = null);
        void Update(TEntity entity);
        void Save();
    }

    interface IRepository2<TEntity>
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null);
        TEntity GetByID(Expression<Func<TEntity, bool>> filter = null);
        void Insert(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Save();
    }

    interface IUser : IRepository2<CA_Usuarios>
    {
    }
}

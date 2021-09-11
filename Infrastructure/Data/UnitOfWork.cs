using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext e_context;

        private Hashtable e_repositories;
        public UnitOfWork(StoreContext context)
        {
            e_context = context;
        }

        public async Task<int> Complete()
        {
           return await e_context.SaveChangesAsync(); 
        }

        public void Dispose()
        {
            e_context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
           if(e_repositories == null) e_repositories = new Hashtable();

           var type  = typeof(TEntity).Name;

           if(!e_repositories.ContainsKey(type))
           {
               var repositoryType = typeof(GenericRespository<>);
               var repositoryInstance = Activator.
                            CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)),e_context);

               e_repositories.Add(type,repositoryInstance);
           }

           return (IGenericRepository<TEntity>) e_repositories[type];
        }
    }
}
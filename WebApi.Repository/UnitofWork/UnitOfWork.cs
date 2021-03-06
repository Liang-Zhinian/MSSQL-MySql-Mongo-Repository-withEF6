﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Repository.UnitofWork
{
    class UnitOfWork : IDisposable
    {
        private DbContext _context;
        private Dictionary<string, object> repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            repositories = new Dictionary<string, object>();
        }

        public TRepo GetRepo<TRepo>() where TRepo : class
        {
            var type = typeof(TRepo).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryInstance = (TRepo)Activator.CreateInstance(typeof(TRepo), new object[] { _context });
                repositories.Add(type, repositoryInstance);
            }

            return (TRepo)repositories[type];
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

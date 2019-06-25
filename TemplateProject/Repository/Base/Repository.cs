using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TemplateProject.Repository.Base
{
    public abstract class Repository<T> where T : class
    {
        protected DbContext Db;

        protected DbSet<T> Table
        {
            get { return Db.Set<T>(); }
        }

        protected Repository(DbContext db)
        {
            Db = db;
        }

        public virtual T GetById(long? id)
        {
            return Table.Find(id);
        }

        public virtual T GetById(string id)
        {
            return Table.Find(id);
        }

        public virtual long Count(Func<T, bool> predicate)
        {
            return Table.Count(predicate);
        }

        public virtual long CountAll()
        {
            return Table.Count();
        }

        public virtual List<T> GetAll()
        {
            return Table.ToList();
        }

        public virtual bool Update(T entity)
        {
            Table.Attach(entity);
            Db.Entry(entity).State = EntityState.Modified;
            return Db.SaveChanges() > 0;
        }

        public virtual bool Save(T entity)
        {
            Table.Add(entity);
            return Db.SaveChanges() > 0;
        }

        public virtual bool SaveAll(List<T> entity)
        {
            Table.AddRange(entity);
            return Db.SaveChanges() > 0;
        }

        public virtual bool Remove(T entity)
        {
            Table.Remove(entity);
            return Db.SaveChanges() > 0;
        }

        public virtual bool RemoveAll(List<T> entity)
        {
            Table.RemoveRange(entity);
            return Db.SaveChanges() > 0;
        }

        public T GetFirstOrDefault(Func<T, bool> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }

        public bool IsExistFirstOrDefault(Func<T, bool> predicate)
        {
            return Table.Any(predicate);
        }

        public T GetLastOrDefault(Func<T, bool> predicate)
        {
            return Table.LastOrDefault(predicate);
        }

        public bool IsExistLastOrDefault(Func<T, bool> predicate)
        {
            return Table.Any(predicate);
        }

        public List<T> Get(Func<T, bool> predicate)
        {
            return Table.Where(predicate).ToList();
        }

        public List<T> GetWithRelatedData(Func<T, bool> predicate, string relatedPath)
        {
            return Table.Include(relatedPath).Where(predicate).ToList();
        }

        public List<T> GetAllWithRelatedData(string path)
        {
            return Table.Include(path).ToList();
        }

        public dynamic GetSelectedValueOfSingleRow(Func<T, bool> wherePredicate, Func<T, dynamic> selectDelegate)
        {
            return Table.Where(wherePredicate).Select(selectDelegate).FirstOrDefault();
        }
    }
}

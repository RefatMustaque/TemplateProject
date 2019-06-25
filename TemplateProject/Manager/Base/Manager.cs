using TemplateProject.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateProject.Manager.Base
{
    public abstract class Manager<T> where T : class
    {
        protected Repository<T> Repository { get; }

        protected Manager(Repository<T> repository)
        {
            Repository = repository;
        }

        public virtual string GenerateId(string prefix, int stringLength)
        {
            Random random = new Random();
            string alphabets = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
            string numbers = "123456789";
            string characters = alphabets + numbers;
            int length = 2;
            int no = 2;
            string Key = "";

            for (int j = 0; j < no; j++)
            {
                string item = string.Empty;
                for (int i = 0; i < length; i++)
                {

                    string character = string.Empty;

                    item = new string(Enumerable.Repeat(characters, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());

                }

                Key += item;
            }

            long rowCount = Repository.CountAll() + 1;

            string recordId = prefix + Key + rowCount.ToString("D" + stringLength);

            return recordId;
        }


        public virtual T GetById(long? id)
        {
            return Repository.GetById(id);
        }

        public virtual T GetById(string id)
        {
            return Repository.GetById(id);
        }

        public virtual List<T> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual long Count(Func<T, bool> predicate)
        {
            return Repository.Count(predicate);
        }

        public virtual long CountAll()
        {
            return Repository.CountAll();
        }

        public virtual bool Save(T entity)
        {
            return Repository.Save(entity);
        }

        public virtual bool SaveAll(List<T> entity)
        {
            return Repository.SaveAll(entity);
        }

        public virtual bool Update(T entity)
        {
            return Repository.Update(entity);
        }

        public virtual bool Remove(T entity)
        {
            return Repository.Remove(entity);
        }

        public virtual bool RemoveAll(List<T> entity)
        {
            return Repository.RemoveAll(entity);
        }

        public T GetFirstOrDefault(Func<T, bool> predicate)
        {
            return Repository.GetFirstOrDefault(predicate);
        }

        public bool IsExistFirstOrDefault(Func<T, bool> predicate)
        {
            return Repository.IsExistFirstOrDefault(predicate);
        }

        public T GetLastOrDefault(Func<T, bool> predicate)
        {
            return Repository.GetLastOrDefault(predicate);
        }

        public bool IsExistLastOrDefault(Func<T, bool> predicate)
        {
            return Repository.IsExistFirstOrDefault(predicate);
        }

        public List<T> Get(Func<T, bool> predicate)
        {
            return Repository.Get(predicate);
        }

        public List<T> GetAllWithRelatedData(string path)
        {
            return Repository.GetAllWithRelatedData(path);
        }
    }
}
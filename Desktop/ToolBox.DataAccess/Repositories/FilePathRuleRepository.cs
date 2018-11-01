using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class FilePathRuleRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public FilePathRuleRepository()
        {
        }

        public IEnumerable<FilePathRule> Get()
        {
            return context.FilePathRules.AsEnumerable();
        }

        public FilePathRule GetByID(int id)
        {
            return context.FilePathRules.Find(id);
        }

        public void Insert(FilePathRule obj)
        {
            context.FilePathRules.Add(obj);
        }

        public void Delete(int ID)
        {
            FilePathRule obj = context.FilePathRules.Find(ID);
            context.FilePathRules.Remove(obj);
        }

        public void Update(FilePathRule obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

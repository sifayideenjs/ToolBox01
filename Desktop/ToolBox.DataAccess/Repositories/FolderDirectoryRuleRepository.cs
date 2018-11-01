using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class FolderDirectoryRuleRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public FolderDirectoryRuleRepository()
        {
        }

        public IEnumerable<FolderDirectoryRule> Get()
        {
            return context.FolderDirectoryRules.AsEnumerable();
        }

        public FolderDirectoryRule GetByID(int id)
        {
            return context.FolderDirectoryRules.Find(id);
        }

        public void Insert(FolderDirectoryRule obj)
        {
            context.FolderDirectoryRules.Add(obj);
        }

        public void Delete(int ID)
        {
           FolderDirectoryRule obj = context.FolderDirectoryRules.Find(ID);
            context.FolderDirectoryRules.Remove(obj);
        }

        public void Update(FolderDirectoryRule obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

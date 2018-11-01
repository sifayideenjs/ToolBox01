using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class RuleRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public RuleRepository()
        {
        }

        public IEnumerable<Rule> Get()
        {
            return context.Rules.AsEnumerable();
        }

        public Rule GetByID(int id)
        {
            return context.Rules.Find(id);
        }

        public void Insert(Rule obj)
        {
            context.Rules.Add(obj);
        }

        public void Delete(int ID)
        {
            Rule obj = context.Rules.Find(ID);
            context.Rules.Remove(obj);
        }

        public void Update(Rule obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

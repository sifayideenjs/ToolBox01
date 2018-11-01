using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class RuleSetRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public RuleSetRepository()
        {
        }

        public IEnumerable<RuleSet> Get()
        {
            return context.RuleSets.AsEnumerable();
        }

        public RuleSet GetByID(int id)
        {
            return context.RuleSets.Find(id);
        }

        public void Insert(RuleSet obj)
        {
            context.RuleSets.Add(obj);
        }

        public void Delete(int ID)
        {
            RuleSet obj = context.RuleSets.Find(ID);
            context.RuleSets.Remove(obj);
        }

        public void Update(RuleSet obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

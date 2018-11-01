using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class TextRuleRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public TextRuleRepository()
        {
        }

        public IEnumerable<TextRule> Get()
        {
            return context.TextRules.AsEnumerable();
        }

        public TextRule GetByID(int id)
        {
            return context.TextRules.Find(id);
        }

        public void Insert(TextRule obj)
        {
            context.TextRules.Add(obj);
        }

        public void Delete(int ID)
        {
            TextRule obj = context.TextRules.Find(ID);
            context.TextRules.Remove(obj);
        }

        public void Update(TextRule obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

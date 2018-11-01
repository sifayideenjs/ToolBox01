using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class HubSectionRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public HubSectionRepository()
        {
        }

        public IEnumerable<HubSection> Get()
        {
            return context.HubSections.AsEnumerable();
        }

        public HubSection GetByID(int id)
        {
            return context.HubSections.Find(id);
        }

        public void Insert(HubSection obj)
        {
            context.HubSections.Add(obj);
        }

        public void Delete(int ID)
        {
            HubSection obj = context.HubSections.Find(ID);
            context.HubSections.Remove(obj);
        }

        public void Update(HubSection obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

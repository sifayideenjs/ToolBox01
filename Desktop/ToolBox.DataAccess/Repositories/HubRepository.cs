using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class HubRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public HubRepository()
        {
        }

        public IEnumerable<Hub> Get()
        {
            return context.Hubs.AsEnumerable();
        }

        public Hub GetByID(int id)
        {
            return context.Hubs.Find(id);
        }

        public void Insert(Hub obj)
        {
            context.Hubs.Add(obj);
        }

        public void Delete(int ID)
        {
            Hub obj = context.Hubs.Find(ID);
            context.Hubs.Remove(obj);
        }

        public void Update(Hub obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

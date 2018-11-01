using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class CenterOfExcellenceRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public CenterOfExcellenceRepository()
        {
        }

        public IEnumerable<CenterOfExcellence> Get()
        {
            return context.CenterOfExcellences.AsEnumerable();
        }

        public CenterOfExcellence GetByID(int id)
        {
            return context.CenterOfExcellences.Find(id);
        }

        public void Insert(CenterOfExcellence obj)
        {
            try
            {
                context.CenterOfExcellences.Add(obj);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        //raise a new exception inserting the current one as the InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                foreach (var entity in e.Entries)
                {
                    string message = entity.Entity.ToString();
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                throw;
            }
        }

        public void Delete(int ID)
        {
            CenterOfExcellence obj = context.CenterOfExcellences.Find(ID);
            context.CenterOfExcellences.Remove(obj);
        }

        public void Update(CenterOfExcellence obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

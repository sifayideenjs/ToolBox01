using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess.Repositories
{
    public class TileRepository
    {
        private ToolBoxContext context = ToolBoxContextSingleton.Instance;

        public TileRepository()
        {
        }

        public IEnumerable<Tile> Get()
        {
            return context.Tiles.AsEnumerable();
        }

        public Tile GetByID(int id)
        {
            return context.Tiles.Find(id);
        }

        public void Insert(Tile obj)
        {
            context.Tiles.Add(obj);
        }

        public void Delete(int ID)
        {
            Tile obj = context.Tiles.Find(ID);
            context.Tiles.Remove(obj);
        }

        public void Update(Tile obj)
        {
            //context.Set<Tile>().Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

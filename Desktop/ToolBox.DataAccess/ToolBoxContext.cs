using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.DataAccess
{
    public class ToolBoxContext : DbContext
    {
        public ToolBoxContext() : base("name=ToolBoxContext")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());

            Database.SetInitializer<ToolBoxContext>(new CreateDatabaseIfNotExists<ToolBoxContext>());
            //Database.SetInitializer<ToolBoxContext>(new DropCreateDatabaseIfModelChanges<ToolBoxContext>());
            //Database.SetInitializer<ToolBoxContext>(new DropCreateDatabaseAlways<ToolBoxContext>());
            //Database.SetInitializer<ToolBoxContext>(new POSDBInitializer());
        }

        public DbSet<CenterOfExcellence> CenterOfExcellences { get; set; }
        public DbSet<Hub> Hubs { get; set; }
        public DbSet<HubSection> HubSections { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<RuleSet> RuleSets { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<TextRule> TextRules { get; set; }
        public DbSet<FilePathRule> FilePathRules { get; set; }
        public DbSet<FolderDirectoryRule> FolderDirectoryRules { get; set; }
    }
}

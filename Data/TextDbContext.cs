using CollaborationApp.Models.Domain;
using EntitySignal.Server.EFDbContext.Data;
using EntitySignal.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollaborationApp.Data
{
    public class TextDbContext : EntitySignalIdentityDbContext
    {
        //will watch for changes and then notify 
        public TextDbContext(DbContextOptions options  , EntitySignalDataProcess entitySignalDataProcess) : base(options, entitySignalDataProcess)
        {
        }

        public DbSet<Text> Text{ get; set; }
    }

}
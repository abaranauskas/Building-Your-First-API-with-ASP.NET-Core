using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class CityInfoContext :DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) 
            :base(options)
        {
            //Database.EnsureCreated(); jei nera bazes sukurs
            Database.Migrate(); // execute migration jei nera sukursvietoj update-database
        }


        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterest { get; set; }


        //vienas is budu conectinti su Db
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Conection string");

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}

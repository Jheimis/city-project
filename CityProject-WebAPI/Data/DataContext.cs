using System.Collections.Generic;
using CityProject_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CityProject_WebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) { } 
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet <ParametroCusto> ParametroCustos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           
           builder.Entity<Estado>()
                .HasData(new List<Estado>(){
                    new Estado(1, "Rio Grande do Sul", 1488252, 161892044.04),
                    new Estado(2, "Santa Catarina", 508826, 55849591.02),
                    new Estado(3, "Paraná", 575377, 63055067.79),
                });

            builder.Entity<Cidade>()
                .Property(c => c.Populacao)
                .IsRequired();

            builder.Entity<Cidade>()
                .HasData(new List<Cidade>(){
                    new Cidade( 1, "Porto Alegre", 1488252, 161892044.04, 1),
                    new Cidade(2, "Florianópolis", 508826, 55849591.02, 2),
                    new Cidade(3, "Londrina", 575377, 63055067.79, 3),
                });

            builder.Entity<ParametroCusto>()
                .HasNoKey();
                
            
        }
    }
}
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
                    new Estado(1, "Rio Grande do Sul", 1488252, 161126570.1438, null),
                    new Estado(2, "Santa Catarina", 508826, 55088377.6269, null),
                    new Estado(3, "Paraná", 575377, 62293564.90005, null),
                });

            builder.Entity<Cidade>()
                .Property(c => c.Populacao)
                .IsRequired();

            builder.Entity<Cidade>()
                .HasData(new List<Cidade>(){
                    new Cidade(1, "Porto Alegre", 1488252, 161126570.1438, 1, null),
                    new Cidade(2, "Florianópolis", 508826, 55088377.6269, 2, null),
                    new Cidade(3, "Londrina", 575377, 62293564.90005, 3, null),
                });

            builder.Entity<ParametroCusto>()
                .HasNoKey();
        }
    }
}
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
                    new Estado(1, "Rio Grande do Sul", 1488252, 16189204404, null),
                    new Estado(2, "Santa Catarina", 508826, 5584959102, null),
                    new Estado(3, "Paraná", 575377, 6305506779, null),
                });

            builder.Entity<Cidade>()
                .Property(c => c.Populacao)
                .IsRequired();

            builder.Entity<Cidade>()
                .HasData(new List<Cidade>(){
                    new Cidade(1, "Porto Alegre", 1488252, 16189204404, 1, null),
                    new Cidade(2, "Florianópolis", 508826, 5584959102, 2, null),
                    new Cidade(3, "Londrina", 575377, 6305506779, 3, null),
                });

            builder.Entity<ParametroCusto>()
                .HasNoKey();
        }
    }
}
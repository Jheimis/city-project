using System.Collections.Generic;

namespace CityProject_WebAPI.Models
{
    public class Estado
    {
        public Estado()
        {
            
        }
        public Estado(int id, string nome, int populacao, double custoEstadoUS)
        {
            this.Id = id;
            this.Nome = nome;
            this.Populacao = populacao;
            this.CustoEstadoUS = custoEstadoUS;

        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Populacao { get; set; }
        public double CustoEstadoUS { get; set; }
        public ICollection<Cidade> Cidades { get; set; }
    }
}
namespace CityProject_WebAPI.Models
{
    public class Cidade
    {
        public Cidade()
        {

        }

        public Cidade(int id, string nome, int populacao, double custoCidadeUS, int estadoId)
        {
            this.Id = id;
            this.Nome = nome;
            this.Populacao = populacao;
            this.CustoCidadeUS = custoCidadeUS;
            this.EstadoId = estadoId;

        }
        
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Populacao { get; set; }
        public double CustoCidadeUS { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }

    }
}
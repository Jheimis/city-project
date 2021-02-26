namespace CityProject_WebAPI.Models
{
    public class ParametroCusto
    {
        public ParametroCusto()
        {
            
        }
        public ParametroCusto(double porPessoa, int valorCorte, double desconto)
        {
            this.PorPessoa = porPessoa;
            this.ValorCorte = valorCorte;
            this.Desconto = desconto;

        }
        public double PorPessoa { get; set; }
        public int ValorCorte { get; set; }
        public double Desconto { get; set; }

    }
}
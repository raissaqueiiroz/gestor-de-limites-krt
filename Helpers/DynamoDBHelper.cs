namespace gestor_de_limitres_krt.Helpers
{
    public class DynamoDBHelper
    {
        public static string  FormataChaveValorCpfBanco(string cpf)
        {
            const string prefixoPKCpf = "CPF#";

            string chaveFormatada = $"{prefixoPKCpf}{cpf}";

            return chaveFormatada;
        }

        public static string FormataChaveValorIsTransacao(string transacaoOuCpf)
        {
            if (transacaoOuCpf != "transacao")
            {
                const string prefixoSKDadosCliente = "DADOSCLI#CPF#";

                string chaveFormatada = $"{prefixoSKDadosCliente}{transacaoOuCpf}";

                return chaveFormatada;
            }

            else
            {
                string id = GeradorUniqueId();

                const string prefiroSKTransacao = "TRANSACAO#";

                string chaveFormatada = $"{prefiroSKTransacao}{id}";

                return chaveFormatada;
            }
        }

        public static string GeradorUniqueId()
        {
            Guid uniqueId = Guid.NewGuid();
            
            return uniqueId.ToString();
        }

        public static double ConverteStringToDouble(string valor)
        {
            return double.Parse(valor);
        }

        public static double CalculaLimiteTransferencia(double valorTransferencia, double valorLimite)
        {
            return valorLimite - valorTransferencia;
        }
    }
}

using Amazon.DynamoDBv2.DataModel;

namespace gestor_de_limitres_krt.Models
{
    [DynamoDBTable("gestao-de-limites-table")]
    public class GestorDeLimitesModel
    {
        [DynamoDBHashKey("documento")]
        public string? Documento { get; set; }

        [DynamoDBRangeKey("consulta")]
        public string? Consulta { get; set; }

        [DynamoDBProperty("dataTransacao")]
        public string? Data_Transacao { get; set; }

        [DynamoDBProperty("limitePix")]
        public double? Limite_Pix { get; set; }

        [DynamoDBProperty("numeroAgencia")]
        public string? Numero_Agencia { get; set;}

        [DynamoDBProperty("numeroConta")]
        public string? Numero_Conta { get; set; }

        [DynamoDBProperty("statusTransacao")]
        public string? Status_Transacao { get; set; }

        [DynamoDBProperty("valorTransacao")]
        public double? Valor_Transacao { get; set; }

        [DynamoDBProperty("destinoTransacao")]
        public string? Destino_Transacao { get; set; }
    }
}

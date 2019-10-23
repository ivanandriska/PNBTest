using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Confluent.Kafka;
using System.Data;

namespace CaseBusiness.CC.Mensageria
{
    public class EventoContato
    {
        public Int32 ID_CAMPANHA { get; set; }

        public String ID_CLIENTE { get; set; }

        public String NOM_CANAL { get; set; }

        public String ID_MSG { get; set; }

        public String COD_EVENTO { get; set; }

        public String DTH_EVENTO { get; set; }

        public String COD_RETORNO { get; set; }

        public String TXT_MSG_RETORNO { get; set; }

        public String DTH_RETORNO { get; set; }

        public Int32 ID_CANAL { get; set; }

        public Int32 COD_LOTE { get; set; }

        public String ID_MSG_EXT { get; set; }

        public String DS_OPERADORA { get; set; }

        public String dh_relatorio { get; set; }

        public String operation { get; set; }

        public Int32 operation_sequence { get; set; }

        public String hashKey { get; set; }

        public Int32 productionDate { get; set; }

        public String source { get; set; }

        public void EnviarKafka()
        {
            String eventoContatoSerializado = JsonConvert.SerializeObject(this);
            String KafkaBootstrapServers = new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.KAFKA_SERVERS, 0).Valor;
            String SMSContactEventTopic = new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.SMSRetCntctEvntTopic, 0).Valor;

            var config = new ProducerConfig {
                BootstrapServers = KafkaBootstrapServers,
                RequestTimeoutMs = 60000,
                MessageSendMaxRetries = 20,
                EnableIdempotence = true,
                Acks = Acks.All,
                MaxInFlight = 5,
                CompressionType = CompressionType.Snappy,
                LingerMs = 5000
            };

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                var dr = p.ProduceAsync(SMSContactEventTopic, new Message<Null, string> { Value = eventoContatoSerializado });
                //var dr = await p.ProduceAsync(SMSA.AppSettings["SMSContactEventTopic"].ToString(), new Message<Null, string> { Value = "test" });
            }
        }
    }
}

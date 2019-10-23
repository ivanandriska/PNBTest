using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Confluent.Kafka;
using System.Data;

namespace CaseBusiness.CC.Mensageria
{
    public class MensagemRequisicao
    {
        public Int32 ID_CAMPANHA { get; set; }

        public String ID_MSG { get; set; }

        public String ID_CLIENTE { get; set; }

        public String NUM_CPF { get; set; }

        public String NOM_CLIENTE { get; set; }

        public Int32 COD_LOTE { get; set; }

        public String FLG_TESTE { get; set; }

        public Int32 ID_CANAL { get; set; }

        public String DES_DESTINATARIO { get; set; }

        public Int32 ID_CONF_COM { get; set; }

        public dynamic TAGS { get; set; }
    }
}

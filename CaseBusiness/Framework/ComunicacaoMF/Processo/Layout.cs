using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using CaseBusiness.Framework.ComunicacaoMF.Interface;

namespace CaseBusiness.Framework.ComunicacaoMF.Processo
{
    internal class Layout
    {
        private Int32 pos = 0;

        internal String GerarMensagem(IEntidade entidade, String chaveAcao)
        {
            Entidade.Layout.Acao layoutAcao;
            String msg = "";

            layoutAcao = Buscar(entidade.GetType().Name, chaveAcao, Tipo.Envio);

            if (layoutAcao != null)
            {
                ((Entidade.Header)entidade).Programa = layoutAcao.Programa;
                ((Entidade.Header)entidade).RegistrosPorPagina = layoutAcao.RegistrosPorPagina;
                
                msg = MontarHeader(entidade);
                msg += MontarMensagem(entidade, layoutAcao);
            }

            return msg;
        }

        private Int32 TamanhoMensagem(Entidade.Layout.Acao layoutAcao)
        {
            Int32 tamanho = 0;

            foreach (Entidade.Layout.Campo c in layoutAcao.Campos)
                tamanho += c.Tamanho;

            return tamanho;

        }

        internal IEntidade LerMensagem(IEntidade entidade, String chaveAcao, String msg)
        {
            Entidade.Layout.Acao layoutAcao;
            pos = 0;

            layoutAcao = Buscar(entidade.GetType().Name, chaveAcao, Tipo.Retorno);

            if (layoutAcao != null)
            {
                entidade = DesmontarHeader(entidade, msg);
                entidade = DesmontarMensagem(entidade, layoutAcao, msg);
            }

            return entidade;
        }

        internal List<IEntidade> LerMensagens<T>(T entidade, String chaveAcao, String msg) where T : IEntidade, new()
        {
            Entidade.Layout.Acao layoutAcao;
            IEntidade item;
            Type t = entidade.GetType();

            List<IEntidade> colTelas = new List<IEntidade>();

            pos = 0;

            layoutAcao = Buscar(t.Name, chaveAcao, Tipo.Retorno);

            if (layoutAcao != null)
            {
                while (pos < msg.Length)
                {
                    item = new T();
                    
                    if (pos == 0)
                        item = DesmontarHeader(item, msg);

                    item = DesmontarMensagem(item, layoutAcao, msg);

                    colTelas.Add(item);
                }
            }

            return colTelas;
        }

        #region Desmontar

        private IEntidade DesmontarHeader(IEntidade tela, String msg)
        {
            foreach (Entidade.Layout.Campo campo in Entidade.Layout.Mapa.Header.Campos)
            {
                ((Entidade.Header)tela).GetType().GetProperty(campo.Propriedade).SetValue(tela, DesmontarCampo(campo.TipoDados, msg.Substring(pos, campo.Tamanho), campo.Formato), null);
                pos += campo.Tamanho;
            }

            pos += 5;

            return tela;
        }

        private IEntidade DesmontarMensagem<T>(T tela, Entidade.Layout.Acao layoutAcao, String msg) where T : IEntidade
        {
            if (pos < msg.Length)
            {
                foreach (Entidade.Layout.Campo campo in layoutAcao.Campos)
                {
                    tela.GetType().GetProperty(campo.Propriedade).SetValue(tela, DesmontarCampo(campo.TipoDados, msg.Substring(pos, campo.Tamanho), campo.Formato), null);
                    pos += campo.Tamanho;
                }
            }

            return tela;
        }

        private Object DesmontarCampo(TipoDados tipoDados, String valor, String formato)
        {
            Object v = new Object();

            try
            {
                switch (tipoDados)
                {
                    case TipoDados.Int:
                        if (valor.Trim().Length > 0)
                            v = Convert.ToInt32(valor);
                        else
                            v = 0;
                        break;
                    case TipoDados.String:
                        v = valor.Trim();
                        break;
                    case TipoDados.Date:
                        if (valor.Trim().Length > 0)
                            v = DesmontarDataHora(valor, formato);
                        else
                            v = DateTime.MinValue;
                        break;
                    case TipoDados.Time:
                        if (valor.Trim().Length > 0)
                            v = DesmontarDataHora(valor, formato);
                        else
                            v = DateTime.MinValue;
                        break;
                    case TipoDados.BigInt:
                        if (valor.Trim().Length > 0)
                            v = Convert.ToInt64(valor);
                        else
                            v = 0;
                        break;
                }
            }
            catch (System.Exception)
            {
                switch (tipoDados)
                {
                    case TipoDados.Int:
                        v = Int32.MinValue;
                        break;
                    case TipoDados.String:
                        v = "";
                        break;
                    case TipoDados.Date:
                        v = DateTime.MinValue;
                        break;
                    case TipoDados.Time:
                        v = DateTime.MinValue;
                        break;
                    case TipoDados.BigInt:
                        v = Int64.MinValue;
                        break;
                }
            }

            return v;
        }

        private DateTime DesmontarDataHora(String valor, String formato)
        {
            Int32 posInicial = 0;
            Int32 posFinal = 0;
            DateTime dtFinal;
            String[] cFormato = new String[6] {"y", "M", "d", "H", "m", "s"};
            Int32[] vFormato = new Int32[6] { 1, 1, 1, 0, 0, 0 };

            if (Convert.ToInt32(valor) > 0)
            {
                for (Int32 p = 0; p < cFormato.Length; p++)
                {
                    posInicial = formato.IndexOf(cFormato[p]);
                    posFinal = formato.LastIndexOf(cFormato[p]);
                    if (posInicial >= 0 && posFinal >= 0)
                        vFormato[p] = Convert.ToInt32(valor.Substring(posInicial, (posFinal - posInicial) + 1));
                }
            }

            dtFinal = new DateTime(vFormato[0], vFormato[1], vFormato[2], vFormato[3], vFormato[4], vFormato[5]);

            return dtFinal;

        }
        
        #endregion Desmontar

        #region Montar

        private String MontarHeader(IEntidade tela)
        {
            String header = "";
            Type t = tela.GetType();

            foreach (Entidade.Layout.Campo campo in Entidade.Layout.Mapa.Header.Campos)
                header += MontarCampo(campo.TipoDados, (Object)t.GetProperty(campo.Propriedade).GetValue(tela, null), campo.Tamanho, campo.Formato);

            return header;
        }

        private String MontarMensagem(IEntidade tela, Entidade.Layout.Acao layoutAcao)
        {
            String msg = "";
            Type t = tela.GetType();

            foreach (Entidade.Layout.Campo campo in layoutAcao.Campos)
                msg += MontarCampo(campo.TipoDados, (Object)t.GetProperty(campo.Propriedade).GetValue(tela, null), campo.Tamanho, campo.Formato);

            msg = msg.Length.ToString().PadLeft(5,'0') + msg;
            
            return msg;
        }

        private String MontarCampo(TipoDados tipoDados, Object valor, Int32 tamanho, String formato)
        {
            String v = "";

            switch (tipoDados)
            {
                case TipoDados.Int:
                    v = MontarNumerico(Convert.ToInt32(valor), tamanho);
                    break;
                case TipoDados.String:
                    v = MontarAlfanumerico(valor.ToString(), tamanho);
                    break;
                case TipoDados.Date:
                    v = MontarDataHora(Convert.ToDateTime(valor), formato);
                    break;
                case TipoDados.Time:
                    v = MontarDataHora(Convert.ToDateTime(valor), formato);
                    break;
                case TipoDados.BigInt:
                    v = MontarNumericoLongo(Convert.ToInt64(valor), tamanho);
                    break;
            }

            return v;
        }

        private String MontarAlfanumerico(String valor, Int32 tamanho)
        {
            String v = valor.Trim().PadRight(tamanho, ' ').ToUpper();
            v = RemoverAcentosECaracteresEspeciais(v);
            
            return v;
        }

        private String MontarNumerico(Int32 valor, Int32 tamanho)
        {
            String v = "";

            if (valor == Int32.MinValue)
                v = v.PadLeft(tamanho, '0');
            else
            {
                v = valor.ToString().Replace(",", "").Replace(".", "");
                v = v.PadLeft(tamanho, '0');
            }
            
            return v;
        }

        private String MontarNumericoLongo(Int64 valor, Int32 tamanho)
        {
            String v = "";

            if (valor == Int64.MinValue)
                v = v.PadLeft(tamanho, '0');
            else
            {
                v = valor.ToString().Replace(",", "").Replace(".", "");
                v = v.PadLeft(tamanho, '0');
            }

            return v;
        }

        private String MontarDataHora(DateTime valor, String formato)
        {
            String v = "";

            if (valor == DateTime.MinValue)
                v = v.PadLeft(formato.Length, '0');
            else
                v = valor.ToString(formato);
            
            return v;
        }
        #endregion Montar

        #region Generico

        private String RemoverAcentosECaracteresEspeciais(String texto)
        {
            String textor = "";

            for (int i = 0; i < texto.Length; i++)
            {
                if (texto[i].ToString() == "ã") textor += "a";
                else if (texto[i].ToString() == "Ã") textor += "A";
                else if (texto[i].ToString() == "Á") textor += "A";
                else if (texto[i].ToString() == "À") textor += "A";
                else if (texto[i].ToString() == "Â") textor += "A";
                else if (texto[i].ToString() == "Ä") textor += "A";
                else if (texto[i].ToString() == "É") textor += "E";
                else if (texto[i].ToString() == "È") textor += "E";
                else if (texto[i].ToString() == "Ê") textor += "E";
                else if (texto[i].ToString() == "Ë") textor += "E";
                else if (texto[i].ToString() == "Í") textor += "I";
                else if (texto[i].ToString() == "Ì") textor += "I";
                else if (texto[i].ToString() == "Ï") textor += "I";
                else if (texto[i].ToString() == "Õ") textor += "O";
                else if (texto[i].ToString() == "Ó") textor += "O";
                else if (texto[i].ToString() == "Ò") textor += "O";
                else if (texto[i].ToString() == "Ö") textor += "O";
                else if (texto[i].ToString() == "Ú") textor += "U";
                else if (texto[i].ToString() == "Ù") textor += "U";
                else if (texto[i].ToString() == "Ü") textor += "U";
                else if (texto[i].ToString() == "Ç") textor += "C";
                else textor += texto[i];
            }
            return textor;
        }

        private Entidade.Layout.Acao Buscar(String telaNome, String chaveAcao, Tipo tipo)
        {
            Entidade.Layout.Acao layoutAcao = null;

            foreach (Entidade.Layout.Mensagem tela in Entidade.Layout.Mapa.Mensagens)
            {
                if (tela.Nome == telaNome)
                {
                    foreach (Entidade.Layout.Acao acao in tela.Acao)
                    {
                        if (acao.Nome == chaveAcao && acao.Tipo == tipo)
                        {
                            layoutAcao = acao;
                            break;
                        }
                    }
                }
            }
            return layoutAcao;
        }

        #endregion Generico
    }
}

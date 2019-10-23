using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CaseBusiness.Framework.ComunicacaoMF.Processo
{
    public static class Mapa
    {
        internal static void Carregar(String caminhoLayout)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(caminhoLayout);

                foreach (XmlNode no in xml.DocumentElement.ChildNodes)
                {
                    if (no.NodeType == XmlNodeType.Element)
                    {
                        if (no.Name == "Header")
                        {
                            foreach (XmlNode campo in no.ChildNodes)
                            {
                                CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Campo campoHeader = new CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Campo();

                                campoHeader.Nome = campo.Attributes["nome"].Value;
                                campoHeader.TipoDados = (TipoDados)Convert.ToInt32(campo.Attributes["tipoDados"].Value);
                                campoHeader.Tamanho = Convert.ToInt32(campo.Attributes["tamanho"].Value);
                                campoHeader.Propriedade = campo.Attributes["propriedade"].Value;

                                CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Mapa.Header.Campos.Add(campoHeader);
                            }
                        }
                        else
                        {
                            CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Mensagem msg = new CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Mensagem();
                            msg.Nome = no.Name;

                            foreach (XmlNode acao in no.ChildNodes)
                            {
                                CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Acao acaoMsg = new CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Acao();

                                acaoMsg.Nome = acao.Attributes["nome"].Value;

                                if (acao.Attributes["programa"] != null)
                                    acaoMsg.Programa = acao.Attributes["programa"].Value;

                                acaoMsg.Tipo = (Tipo)Convert.ToInt32(acao.Attributes["tipo"].Value);

                                if (acao.Attributes["registrosPorPagina"] != null)
                                    acaoMsg.RegistrosPorPagina = Convert.ToInt32(acao.Attributes["registrosPorPagina"].Value);

                                foreach (XmlNode campo in acao.ChildNodes)
                                {
                                    CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Campo campoMsg = new CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Campo();

                                    campoMsg.Nome = campo.Attributes["nome"].Value;
                                    campoMsg.TipoDados = (TipoDados)Convert.ToInt32(campo.Attributes["tipoDados"].Value);
                                    campoMsg.Tamanho = Convert.ToInt32(campo.Attributes["tamanho"].Value);
                                    campoMsg.Propriedade = campo.Attributes["propriedade"].Value;

                                    if (campo.Attributes["formato"] != null)
                                        campoMsg.Formato = campo.Attributes["formato"].Value;

                                    acaoMsg.Campos.Add(campoMsg);
                                }
                                msg.Acao.Add(acaoMsg);
                            }
                            CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Mapa.Mensagens.Add(msg);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}

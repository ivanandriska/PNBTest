using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Conexao = CaseBusiness.Framework.Comunicacao.Conexao;
using StatusConexao = CaseBusiness.Framework.Comunicacao.Status.StatusComunicacao;

namespace CaseBusiness.Framework.Comunicacao
{
    public class Comunicacao
    {
        private String _host;
        private Int32 _port;
        private Int32 _receiveTimeout;
        private Boolean _manterAtiva = false;

        public Comunicacao() { }

        public Comunicacao(String host, Int32 port, Int32 receiveTimeOut, Boolean manterAtiva)
        {
            _host = host;
            _port = port;
            _receiveTimeout = receiveTimeOut;
            _manterAtiva = manterAtiva;
        }

        private Boolean Conectar()
        {
            Int32 ContadorFalhaTentativasConexao = 0;

            if (Conexao.StatusConexao == Status.StatusComunicacao.Desconectado)
            {
                IPAddress ipAddr = null;

                String[] pIP = _host.Split('.');
                Byte[] ip = new Byte[4];

                ip[0] = Convert.ToByte(pIP[0]);
                ip[1] = Convert.ToByte(pIP[1]);
                ip[2] = Convert.ToByte(pIP[2]);
                ip[3] = Convert.ToByte(pIP[3]);

                ipAddr = new IPAddress(ip);

                IPEndPoint ipe = new IPEndPoint(ipAddr, _port);

                Conexao.Socket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                Conexao.Socket.ReceiveTimeout = _receiveTimeout;

                while (!Conexao.Socket.Connected)
                {
                    try
                    {
                        Conexao.Socket.Connect(ipe);
                        if (Conexao.Socket.Connected)
                            Conexao.StatusConexao = Status.StatusComunicacao.Conectado;

                        return true;
                    }
                    catch (SocketException)
                    {
                        ContadorFalhaTentativasConexao++;

                        if (ContadorFalhaTentativasConexao == 3)
                            return false;
                    }
                }
            }
            return false;
        }

        public void Desconectar()
        {
            Conexao.Socket.Close();
            Conexao.StatusConexao = StatusConexao.Desconectado;
        }

        private Byte[] Formatar(String mensagem)
        {
            Byte[] msg;
            Byte[] buf;

            msg = Encoding.ASCII.GetBytes(mensagem);

            buf = new Byte[msg.Length + 2];
            Array.Copy(msg, 0, buf, 2, msg.Length);

            Int32 l = msg.Length;
            String preMessageLength = l.ToString("X").PadLeft(4, '0');

            buf[0] = Convert.ToByte(Convert.ToInt32(preMessageLength.Substring(0, 2),16));
            buf[1] = Convert.ToByte(Convert.ToInt32(preMessageLength.Substring(2, 2),16));

            return buf;
        }

        public Boolean Enviar(String mensagem)
        {
            try
            {
                if (Conectar())
                {
                    Byte[] msgFormated = Formatar(mensagem);

                    while (1 == 1)
                    {
                        if (Conexao.StatusConexao == Status.StatusComunicacao.Conectado)
                        {
                            Conexao.Socket.Send(msgFormated, msgFormated.Length, 0);
                            return true;
                        }
                        else
                            Thread.Sleep(1000);
                    }
                }
                return false;
            }
            catch (System.Exception)
            {
                Desconectar();
                return false;
            }
        }

        public String Receber()
        {
            Byte[] bytesReceived = new Byte[1];
            Byte[] bytesMessage = new Byte[0];
            Int32 pos = 0;
            String preMessageLength = "";
            Int32 messageLength = 0;
            String msgReceived = "";

            try
            {
                while (1 == 1)
                {
                    if (Conexao.StatusConexao == Status.StatusComunicacao.Conectado)
                    {
                        if (Conexao.Socket.Available > 0)
                            Conexao.Socket.Receive(bytesReceived, 0, bytesReceived.Length, SocketFlags.None);
                        else
                        {
                            // Efetua a interrupção do processamento até que uma mensagem seja recebida
                            // Esta instrução impede que a aplicação fique ocupando memória e processador desnecessariamente
                            Conexao.Socket.Receive(bytesReceived, 0, 0, SocketFlags.None);

                            if (Conexao.Socket.Available > 0)
                                Conexao.Socket.Receive(bytesReceived, 0, bytesReceived.Length, SocketFlags.None);
                        }

                        pos++;

                        if (pos <= 2)
                        {
                            preMessageLength += bytesReceived[0].ToString("X").PadLeft(2, '0');
                            if (pos == 2)
                                messageLength = Convert.ToInt32(preMessageLength, 16);
                        }
                        else
                        {
                            Array.Resize(ref bytesMessage, bytesMessage.Length + 1);
                            Array.Copy(bytesReceived, 0, bytesMessage, bytesMessage.Length - 1, 1);

                            if ((pos - 2) == messageLength)
                            {
                                msgReceived = Encoding.ASCII.GetString(bytesMessage);
                                if (!_manterAtiva)
                                {
                                    Desconectar();
                                    break;
                                }
                            }
                        }
                    }
                    else
                        Thread.Sleep(1000);
                }
            }
            catch (System.Exception)
            {
                Desconectar();
                throw;
            }

            return msgReceived;
        }

        private void Result(IAsyncResult result)
        {}

        internal void IdentificaComunicacaoAplicacaoLocalRemota()
        {
            try
            {
                CaseBusiness.Framework.BancoDados.Entidade.Configuracao conf = null;
                CaseBusiness.Framework.BancoDados.Processo.Configuracao confProcesso = new CaseBusiness.Framework.BancoDados.Processo.Configuracao();
                conf = confProcesso.BuscarStringConexao(CaseBusiness.Framework.Configuracao.Configuracao.BancoPrincipal);
                Boolean conexaoViaIP = true;

                if (conf != null)
                {
                    IPAddress ipAddr = null;
                    String ipOuNome = "";
                    String host = "";
                    String[] pIP = null;
                    Int32 ipOut = Int32.MinValue;
                    Byte[] ip = new Byte[4];

                    if (conf.Servidor.IndexOf(@"\") > -1)
                        ipOuNome = conf.Servidor.Remove(conf.Servidor.IndexOf(@"\")).Replace(".", "");
                    else
                        ipOuNome = conf.Servidor.Replace(".", "");

                    if (Int32.TryParse(ipOuNome, out ipOut))
                    {
                        if (conf.Servidor.IndexOf(@"\") > -1)
                            pIP = conf.Servidor.Remove(conf.Servidor.IndexOf(@"\")).Split('.');
                        else
                            pIP = conf.Servidor.Split('.');

                        ip[0] = Convert.ToByte(pIP[0]);
                        ip[1] = Convert.ToByte(pIP[1]);
                        ip[2] = Convert.ToByte(pIP[2]);
                        ip[3] = Convert.ToByte(pIP[3]);

                        ipAddr = new IPAddress(ip);

                        conexaoViaIP = true;
                    }
                    else
                    {
                        conexaoViaIP = false;

                        if (conf.Servidor.IndexOf(@"\") > -1)
                            host = conf.Servidor.Remove(conf.Servidor.IndexOf(@"\"));
                        else
                            host = conf.Servidor;
                    }

                    using (System.Net.Sockets.TcpClient tcpSocket = new System.Net.Sockets.TcpClient())
                    {
                        IAsyncResult async = null;
                        
                        if (conexaoViaIP)
                            async = tcpSocket.BeginConnect(ipAddr, Convert.ToInt32(conf.Porta), new AsyncCallback(Result), null);
                        else
                            async = tcpSocket.BeginConnect(host, Convert.ToInt32(conf.Porta), new AsyncCallback(Result), null);
                        
                        DateTime startTime = DateTime.Now;

                        do
                        {
                            System.Threading.Thread.Sleep(500);

                            if (async.IsCompleted)
                                break;
                        }
                        while (DateTime.Now.Subtract(startTime).TotalSeconds < 5);

                        if (async.IsCompleted)
                        {
                            tcpSocket.EndConnect(async);
                            CaseBusiness.Framework.Configuracao.Configuracao._tipoComunicacao = TipoComunicacao.Local;
                        }

                        tcpSocket.Close();

                        if (!async.IsCompleted)
                        {
                            //DataSet ds = null;

                            //CaseWSFramework.FrameworkSoapClient fWS = new CaseWSFramework.FrameworkSoapClient();

                            //CaseWSFramework.App app = CaseWSFramework.App.CaseManagerCliente;
                            //ds = fWS.BuscarConfiguracao(app);

                            //CaseBusiness.Framework.Configuracao.Configuracao._tipoComunicacao = TipoComunicacao.Remota;
                        }
                    }
                }
            }
            catch (SocketException)
            {
                //DataSet ds = null;

                //CaseWSFramework.FrameworkSoapClient fWS = new CaseWSFramework.FrameworkSoapClient();

                //CaseWSFramework.App app = CaseWSFramework.App.CaseManagerCliente;
                //ds = fWS.BuscarConfiguracao(app);

                //CaseBusiness.Framework.Configuracao.Configuracao._tipoComunicacao = TipoComunicacao.Remota;
            }
            catch (System.Exception ex)
            {
				CaseBusiness.Framework.Configuracao.Configuracao._erroInicializacao = "Erro: " + ex.Message;
				CaseBusiness.Framework.Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");

                if (ex.InnerException != null)
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Erro: " + ex.InnerException.ToString() + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }
        }
    }
}
using System;
using System.Net.Sockets;

using StatusConexao = CaseBusiness.Framework.Comunicacao.Status.StatusComunicacao;

namespace CaseBusiness.Framework.Comunicacao
{
    public static class Conexao
    {
        private static Socket _socket;
        private static StatusConexao _statusConexao = StatusConexao.Desconectado;

        public static Socket Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

        public static StatusConexao StatusConexao
        {
            get { return _statusConexao; }
            set { _statusConexao = value; }
        }
    }
}
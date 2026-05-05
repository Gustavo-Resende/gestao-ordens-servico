using System;
using System.IO;

namespace GestaoOrdensServico.Infrastructure.Logging
{
    public class Logger
    {
        private readonly string _logDirectory;
        private readonly object _lock = new object();

        public Logger()
        {
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(_logDirectory);
        }

        public void LogInfo(string mensagem)
        {
            Escrever("INFO", mensagem, null);
        }

        public void LogErro(string mensagem, Exception ex)
        {
            Escrever("ERRO", mensagem, ex);
        }

        private void Escrever(string nivel, string mensagem, Exception ex)
        {
            try
            {
                string nomeArquivo = string.Format("app_{0:yyyy-MM-dd}.log", DateTime.Now);
                string caminho = Path.Combine(_logDirectory, nomeArquivo);

                string linha = string.Format("{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}",
                    DateTime.Now, nivel, mensagem);

                if (ex != null)
                {
                    linha += Environment.NewLine
                        + "  Exceção: " + ex.GetType().Name + ": " + ex.Message
                        + Environment.NewLine
                        + "  StackTrace: " + ex.StackTrace;
                }

                lock (_lock)
                {
                    File.AppendAllText(caminho, linha + Environment.NewLine);
                }
            }
            catch
            {
                // falha no log nunca deve derrubar a aplicação
            }
        }
    }
}

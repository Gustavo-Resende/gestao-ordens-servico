namespace GestaoOrdensServico.Application
{
    public class ResultadoOperacao<T>
    {
        public bool Sucedeu { get; private set; }
        public string Mensagem { get; private set; }
        public T Dados { get; private set; }

        private ResultadoOperacao() { }

        public static ResultadoOperacao<T> Sucesso(T dados)
        {
            return new ResultadoOperacao<T> { Sucedeu = true, Dados = dados };
        }

        public static ResultadoOperacao<T> Falha(string mensagem)
        {
            return new ResultadoOperacao<T> { Sucedeu = false, Mensagem = mensagem };
        }
    }
}

namespace Application.Exceptions
{
    /// <summary>
    /// Excepción personalizada para errores de validación
    /// NO puede llevar InnerException, asegurando mensajes limpios al cliente
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }

        // Sobrescribir ToString() para retornar SOLO el mensaje sin stack trace
        public override string ToString() => this.Message;
    }
}

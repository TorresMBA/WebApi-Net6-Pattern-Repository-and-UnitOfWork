namespace API.Helpers.Errors
{
    public class ApiResponse
    {
        public int StatudCode { get; set; }

        public string Message { get; set; }

        public ApiResponse(int statudCode, string message = null)
        {
            StatudCode = statudCode;
            Message = message ?? GetDefaultMessage(statudCode);
        }

        private string GetDefaultMessage(int statudcode)
        {
            return statudcode switch
            {
                400 => "Haz realizado una petición incorrecta.",
                401 => "Usuario no autorizado.",
                404 => "El recursos que haz intentado solicitar no existe.",
                500 => "Error en el servidor. No eres tú, soy yo. Comunícate con el administrador"
            };
        }
    }
}

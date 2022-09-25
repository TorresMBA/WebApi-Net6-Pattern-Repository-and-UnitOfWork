namespace API.Helpers
{
    public class Autorizacion
    {
        public enum Roles
        {
            Administrador,
            Gerente,
            Empleado,
            Secretaria
        }

        public const Roles rol_predeterminado = Roles.Empleado;
    }
}

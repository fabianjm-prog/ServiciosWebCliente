namespace Cliente_ProyectoFinal.Models
{
    public class ApiResponse<T>
    {
        public string mensaje { get; set; }
        public T value { get; set; }
        public string error { get; set; }
    }
}

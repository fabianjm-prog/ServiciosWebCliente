namespace Cliente_ProyectoFinal.Models.Credito
{
    public class Class_Credito
    {
        public int credito_ID { get; set; }

        public string Cedula_P { get; set; }

        public double monto_maximo { get; set; }

        public double saldo_actual { get; set; }
        public DateTime fecha_creacion { get; set; }

        public DateTime fecha_vencimiento { get; set; }
        public string estado { get; set; }
    }
}

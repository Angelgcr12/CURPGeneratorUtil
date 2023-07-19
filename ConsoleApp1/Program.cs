using CURPUtil;

public class Program
{
    
    public static void Main()
    {
        try
        {
            CURPGenerator generator = new CURPGenerator();
            // Datos de prueba para generar el CURP
            string nombre = "Nombre";
            string primerApellido = "Apellido1";
            string segundoApellido = "Apellido2";
            DateTime fechaNacimiento = new DateTime(2000, 01, 01);
            string sexo = "H";
            string entidadNacimiento = "PUEBLA";

            // Generar el CURP
            string curp = CURPGenerator.GenerarCURP18(nombre, primerApellido, segundoApellido, fechaNacimiento, sexo, entidadNacimiento);

            // Mostrar el CURP generado
            Console.WriteLine($"El CURP generado es: {curp}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

   
}

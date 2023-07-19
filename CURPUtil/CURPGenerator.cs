namespace CURPUtil
{
    public class CURPGenerator
    {
        // Método para generar un CURP de 17 caracteres
        public static string GenerarCURP17(string nombre, string primerApellido, string segundoApellido, DateTime fechaNacimiento, string sexo, string entidadNacimiento)
        {
            // Caracteres que son excepciones en el CURP
            char[] exepciones = { 'Ñ', 'ñ', '/', '.', '-' };

            // Se normalizan y ajustan los datos de entrada
            nombre = AjustarCompuestos(NormalizarTexto(nombre));
            primerApellido = AjustarCompuestos(NormalizarTexto(primerApellido));
            segundoApellido = AjustarCompuestos(NormalizarTexto(segundoApellido));
            fechaNacimiento = NormalizarFecha(fechaNacimiento);
            sexo = NormalizarSexo(sexo);
            entidadNacimiento = NormalizarEntidadFederativa(entidadNacimiento);

            // Se obtienen los caracteres correspondientes para el CURP
            string nombreAUsar = ObtenerNombreValido(nombre);
            char inicialNombre = nombreAUsar[0];
            char inicialApellido1 = exepciones.Contains(primerApellido[0]) ? 'X' : primerApellido[0];
            char vocalApellido1 = ObtenerPrimerVocal(primerApellido);
            char inicialApellido2 = exepciones.Contains(segundoApellido[0]) ? 'X' : segundoApellido[0];
            char consonanteNombre = ObtenerPrimerConsonante(nombreAUsar);
            char consonanteApellido1 = ObtenerPrimerConsonante(primerApellido);
            char consonanteApellido2 = ObtenerPrimerConsonante(segundoApellido);

            // Se obtiene la parte 0-3 del CURP y se filtran las inconveniencias
            string posicion0_3 = FiltrarInconvenientes($"{inicialApellido1}{vocalApellido1}{inicialApellido2}{inicialNombre}");

            // Se obtiene la parte 4-9 del CURP (fecha de nacimiento en formato yyMMdd)
            string posicion4_9 = fechaNacimiento.ToString("yyMMdd");

            // Se obtiene la parte 13-15 del CURP (primer consonante de apellido paterno, primer consonante de apellido materno, primer consonante de nombre)
            string posicion13_15 = $"{consonanteApellido1}{consonanteApellido2}{consonanteNombre}";

            // Se obtiene la parte 16 del CURP (A para nacidos después de 2000, 0 para nacidos antes de 2000)
            char posicion16 = fechaNacimiento.Year > 1999 ? 'A' : '0';

            // Se forma y retorna el CURP de 17 caracteres
            return $"{posicion0_3}{posicion4_9}{sexo}{entidadNacimiento}{posicion13_15}{posicion16}";
        }

        // Método para generar un CURP de 18 caracteres (CURP + dígito verificador)
        public static string GenerarCURP18(string nombre, string primerApellido, string segundoApellido, DateTime fechaNacimiento, string sexo, string entidadNacimiento)
        {
            // Se genera el CURP de 17 caracteres
            string curp = GenerarCURP17(nombre, primerApellido, segundoApellido, fechaNacimiento, sexo, entidadNacimiento);

            // Se calcula el dígito verificador y se agrega al CURP de 17 caracteres
            char digito = CalcularDigitoVerificador(curp);
            return $"{curp}{digito}";
        }

        // Método para calcular el dígito verificador de un CURP
        public static char CalcularDigitoVerificador(string curp)
        {
            string caracteres = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            curp = curp.ToUpper().Substring(0, 17);
            int[] valoresCurp = curp.Select(c => caracteres.IndexOf(c)).ToArray();
            int sumaCurp = valoresCurp.Select((val, i) => val * (18 - i)).Sum();
            int digito = 10 - (sumaCurp % 10);
            digito = digito == 10 ? 0 : digito;

            // Se retorna el dígito verificador calculado
            return caracteres[digito];
        }

        // Método para normalizar una entidad federativa a su clave de dos letras
        public static string NormalizarEntidadFederativa(string entidadFederativa)
        {
            entidadFederativa = entidadFederativa.ToUpper();

            var clavesEntidadesFederativas = new Dictionary<string, string>
            {
                // Diccionario con claves de entidades federativas
                // ...
            };

            if (clavesEntidadesFederativas.TryGetValue(entidadFederativa, out var cvEntidadFederativa))
            {
                return cvEntidadFederativa;
            }
            throw new ArgumentException("La entidad federativa ingresada no es válida");
        }

        // Método para normalizar una fecha a su representación sin hora
        public static DateTime NormalizarFecha(DateTime fechaNacimiento)
        {
            return fechaNacimiento.Date;
        }

        // Método para normalizar el sexo a 'H' o 'M'
        public static string NormalizarSexo(string sexo)
        {
            sexo = sexo.ToUpper().Trim();

            var data = new Dictionary<string, string>
            {
                // Diccionario con valores válidos para el sexo
                // ...
            };

            if (data.TryGetValue(sexo, out var normalizedSexo))
            {
                return normalizedSexo;
            }

            throw new ArgumentException("El valor de sexo no está permitido");
        }

        // Método para filtrar palabras inconvenientes en el CURP
        public static string FiltrarInconvenientes(string texto)
        {
            // Palabras inconvenientes a filtrar
            string[] inconvenientes =
            {
                // ...
            };

            if (inconvenientes.Contains(texto))
            {
                texto = texto.Replace(texto[1], 'X');
            }

            return texto;
        }

        // Método para obtener el nombre válido a usar en el CURP
        public static string ObtenerNombreValido(string nombre)
        {
            string[] nombrePartes = nombre.ToUpper().Trim().Split();
            string[] comunes = { "MARIA", "MA.", "MA", "JOSE", "J", "J." };

            if (nombrePartes.Length > 1 && comunes.Contains(nombrePartes[0]))
            {
                return nombrePartes[1];
            }

            return nombrePartes[0];
        }

        // Método para ajustar nombres compuestos en el CURP
        public static string AjustarCompuestos(string texto)
        {
            string[] textoPartes = texto.ToUpper().Split();
            List<string> textoAux = new List<string>();
            string[] compuestos =
            {
                // ...
            };

            foreach (string item in textoPartes)
            {
                if (!compuestos.Contains(item))
                {
                    textoAux.Add(item);
                }
            }

            return string.Join(" ", textoAux);
        }

        // Método para obtener la primera vocal en un texto
        public static char ObtenerPrimerVocal(string texto)
        {
            string textoUpper = texto.ToUpper().Trim();
            char[] vocales = { 'A', 'E', 'I', 'O', 'U' };
            List<int> aux = new List<int>();

            for (int i = 1; i < textoUpper.Length; i++)
            {
                if (vocales.Contains(textoUpper[i]))
                {
                    aux.Add(i);
                }
            }

            if (aux.Count == 0)
            {
                return 'X';
            }

            return textoUpper[aux.Min()];
        }

        // Método para obtener la primera consonante en un texto
        public static char ObtenerPrimerConsonante(string texto)
        {
            string textoUpper = texto.ToUpper().Trim();
            char[] consonantes = { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'X', 'Y', 'Z', 'Ñ' };
            List<int> aux = new List<int>();

            for (int i = 1; i < textoUpper.Length; i++)
            {
                if (consonantes.Contains(textoUpper[i]))
                {
                    aux.Add(i);
                }
            }

            if (aux.Count == 0)
            {
                return 'X';
            }

            return textoUpper[textoUpper[aux.Min()] == 'Ñ' ? aux.Min() + 1 : aux.Min()];
        }

        // Método para normalizar el texto (quitar acentos y caracteres especiales)
        public static string NormalizarTexto(string texto)
        {
            string[,] data =
            {
                // Diccionario para reemplazar caracteres especiales
                { "Ã", "A" }, { "À", "A" }, { "Á", "A" }, { "Ä", "A" }, { "Â", "A" }, { "È", "E" }, { "É", "E" }, { "Ë", "E" },
                { "Ê", "E" }, { "Ì", "I" }, { "Í", "I" }, { "Ï", "I" }, { "Î", "I" }, { "Ò", "O" }, { "Ó", "O" }, { "Ö", "O" },
                { "Ô", "O" }, { "Ù", "U" }, { "Ú", "U" }, { "Ü", "U" }, { "Û", "U" }, { "ã", "a" }, { "à", "a" }, { "á", "a" },
                { "ä", "a" }, { "â", "a" }, { "è", "e" }, { "é", "e" }, { "ë", "e" }, { "ê", "e" }, { "ì", "i" }, { "í", "i" },
                { "ï", "i" }, { "î", "i" }, { "ò", "o" }, { "ó", "o" }, { "ö", "o" }, { "ô", "o" }, { "ù", "u" }, { "ú", "u" },
                { "ü", "u" }, { "û", "u" }, { "Ç", "c" }, { "ç", "c" }
            };

            for (int i = 0; i < data.GetLength(0); i++)
            {
                texto = texto.Replace(data[i, 0], data[i, 1]);
            }

            return texto;
        }
    }
}

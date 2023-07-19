namespace CURPUtil
{
    public class CURPGenerator
    {
        public static string GenerarCURP17(string nombre, string primerApellido, string segundoApellido, DateTime fechaNacimiento, string sexo, string entidadNacimiento)
        {
            char[] exepciones = { 'Ñ', 'ñ', '/', '.', '-' };

            nombre = AjustarCompuestos(NormalizarTexto(nombre));
            primerApellido = AjustarCompuestos(NormalizarTexto(primerApellido));
            segundoApellido = AjustarCompuestos(NormalizarTexto(segundoApellido));
            fechaNacimiento = NormalizarFecha(fechaNacimiento);
            sexo = NormalizarSexo(sexo);
            entidadNacimiento = NormalizarEntidadFederativa(entidadNacimiento);

            string nombreAUsar = ObtenerNombreValido(nombre);
            char inicialNombre = nombreAUsar[0];
            char inicialApellido1 = exepciones.Contains(primerApellido[0]) ? 'X' : primerApellido[0];
            char vocalApellido1 = ObtenerPrimerVocal(primerApellido);
            char inicialApellido2 = exepciones.Contains(segundoApellido[0]) ? 'X' : segundoApellido[0];
            char consonanteNombre = ObtenerPrimerConsonante(nombreAUsar);
            char consonanteApellido1 = ObtenerPrimerConsonante(primerApellido);
            char consonanteApellido2 = ObtenerPrimerConsonante(segundoApellido);

            string posicion0_3 = FiltrarInconvenientes($"{inicialApellido1}{vocalApellido1}{inicialApellido2}{inicialNombre}");
            string posicion4_9 = fechaNacimiento.ToString("yyMMdd");
            string posicion13_15 = $"{consonanteApellido1}{consonanteApellido2}{consonanteNombre}";
            char posicion16 = fechaNacimiento.Year > 1999 ? 'A' : '0';

            return $"{posicion0_3}{posicion4_9}{sexo}{entidadNacimiento}{posicion13_15}{posicion16}";
        }

        public static string GenerarCURP18(string nombre, string primerApellido, string segundoApellido, DateTime fechaNacimiento, string sexo, string entidadNacimiento)
        {
            string curp = GenerarCURP17(nombre, primerApellido, segundoApellido, fechaNacimiento, sexo, entidadNacimiento);
            char digito = CalcularDigitoVerificador(curp);
            return $"{curp}{digito}";
        }

        public static char CalcularDigitoVerificador(string curp)
        {
            string caracteres = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            curp = curp.ToUpper().Substring(0, 17);
            int[] valoresCurp = curp.Select(c => caracteres.IndexOf(c)).ToArray();
            int sumaCurp = valoresCurp.Select((val, i) => val * (18 - i)).Sum();
            int digito = 10 - (sumaCurp % 10);
            digito = digito == 10 ? 0 : digito;

            return caracteres[digito];
        }

        public static string NormalizarEntidadFederativa(string entidadFederativa)
        {
            entidadFederativa = entidadFederativa.ToUpper();

            var clavesEntidadesFederativas = new Dictionary<string, string>
{
    {"AGUASCALIENTES", "AS"}, {"BAJA CALIFORNIA", "BC"}, {"BAJA CALIFORNIA SUR", "BS"}, {"CAMPECHE", "CC"},
    {"COAHUILA DE ZARAGOZA", "CL"}, {"COLIMA", "CM"}, {"CHIAPAS", "CS"}, {"CHIHUAHUA", "CH"},
    {"CIUDAD DE MEXICO", "DF"}, {"DURANGO", "DG"}, {"GUANAJUATO", "GT"}, {"GUERRERO", "GR"},
    {"HIDALGO", "HG"}, {"JALISCO", "JC"}, {"MEXICO", "MC"}, {"MICHOACAN DE OCAMPO", "MN"},
    {"MORELOS", "MS"}, {"NAYARIT", "NT"}, {"NUEVO LEON", "NL"}, {"OAXACA", "OC"},
    {"PUEBLA", "PL"}, {"QUERETARO", "QT"}, {"QUINTANA ROO", "QR"}, {"SAN LUIS POTOSI", "SP"},
    {"SINALOA", "SL"}, {"SONORA", "SR"}, {"TABASCO", "TC"}, {"TAMAULIPAS", "TS"},
    {"TLAXCALA", "TL"}, {"VERACRUZ", "VZ"}, {"YUCATAN", "YN"}, {"ZACATECAS", "ZS"},
    {"NACIDO EN EL EXTRANJERO", "NE"}
};

            if (clavesEntidadesFederativas.TryGetValue(entidadFederativa, out var cvEntidadFederativa))
            {
                return cvEntidadFederativa;
            }
            throw new ArgumentException("La entidad federativa ingresada no es válida");
        }

        public static DateTime NormalizarFecha(DateTime fechaNacimiento)
        {
            return fechaNacimiento.Date;
        }

        public static string NormalizarSexo(string sexo)
        {
            sexo = sexo.ToUpper().Trim();

            var data = new Dictionary<string, string>
        {
            {"HOMBRE", "H"}, {"MASCULINO", "H"}, {"H", "H"}, {"MUJER", "M"}, {"FEMENINO", "M"}, {"M", "M"}
        };

            if (data.TryGetValue(sexo, out var normalizedSexo))
            {
                return normalizedSexo;
            }

            throw new ArgumentException("El valor de sexo no está permitido");
        }

        public static string FiltrarInconvenientes(string texto)
        {
            string[] inconvenientes =
            {
            "BACA", "BAKA", "BUEI", "BUEY", "CACA", "CACO", "CAGA", "CAGO", "CAKA", "CAKO", "COGE", "COGI",
            "COJA", "COJE", "COJI", "COJO", "COLA", "CULO", "FALO", "FETO", "GETA", "GUEI", "GUEY", "JETA",
            "JOTO", "KACA", "KACO", "KAGA", "KAGO", "KAKA", "KAKO", "KOGE", "KOGI", "KOJA", "KOJE", "KOJI",
            "KOJO", "KOLA", "KULO", "LILO", "LOCA", "LOCO", "LOKA", "LOKO", "MAME", "MAMO", "MEAR", "MEAS",
            "MEON", "MIAR", "MION", "MOCO", "MOKO", "MULA", "MULO", "NACA", "NACO", "PEDA", "PEDO", "PENE",
            "PIPI", "PITO", "POPO", "PUTO", "PUTA", "PUTO", "QULO", "RATA", "ROBA", "ROBE", "ROBO", "RUIN",
            "SENO", "TETA", "VACA", "VAGA", "VAGO", "VAKA", "VUEI", "VUEY", "WUEY", "WUEI", "WUEY"
        };

            if (inconvenientes.Contains(texto))
            {
                texto = texto.Replace(texto[1], 'X');
            }

            return texto;
        }

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

        public static string AjustarCompuestos(string texto)
        {
            string[] textoPartes = texto.ToUpper().Split();
            List<string> textoAux = new List<string>();
            string[] compuestos =
            {
            "DA", "DAS", "DE", "DEL", "DER", "DI", "DIE", "DD", "EL", "LA", "LOS", "LAS", "LE", "LES",
            "MAC", "MC", "VAN", "VON", "Y"
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

        public static string NormalizarTexto(string texto)
        {
            string[,] data =
            {
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
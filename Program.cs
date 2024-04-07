using CsvHelper;
using System.Xml.Serialization;
using System.Globalization;

namespace AC2TractamentDeFitxers
{
    public class Program
    {
        public static void Main()
        {
            string filePath = @"../../../ConsAigua.csv";
            string xmlFilePath = @"../../../ConsAigua.xml";

            //Crear menu para ejecutar los ejercicios
            Console.WriteLine("Selecciona una opció:");
            Console.WriteLine("1. Convertir a XML");
            Console.WriteLine("2. Comarques amb població superior a 200.000 habitants");
            Console.WriteLine("3. Mitjana de consum domèstic per comarca");
            Console.WriteLine("4. Comarques amb el consum domèstic per càpita més alt");
            Console.WriteLine("5. Comarques amb el consum domèstic per càpita més baix");
            Console.WriteLine("6. Filtrar i mostrar comarques");
            int opcion = Convert.ToInt32(Console.ReadLine());

            switch(opcion)
            {
                case 1:
                    convertToXML(filePath, xmlFilePath);
                    break;
                case 2:
                    var records = ReadXML(xmlFilePath);
                    ComarquesPoblacio(records);
                    break;
                case 3:
                     records = ReadXML(xmlFilePath);
                    CalcularMitjanaConsumDomesticPerComarca(records);
                    break;
                case 4:
                    records = ReadXML(xmlFilePath);
                    ComarquesConsumDomesticMesAlt(records);
                    break;
                case 5:
                    records = ReadXML(xmlFilePath);
                    ComarquesConsumDomesticMesBaix(records);
                    break;
                case 6:
                    records = ReadXML(xmlFilePath);
                    Console.WriteLine("Introdueix el nom de comarca o el codi:");
                    string filtro = Console.ReadLine();
                    FiltrarYMostrarComarcas(records, filtro);
                    break;
                default:
                    Console.WriteLine("Opción no válida");
                    break;
            }
        }

        public static void convertToXML(string filePath, string xmlFilePath) // Convertir a XML
        {
            try
            {
                var reader = new StreamReader(filePath);
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<Gestor>().ToList();
                var xml = new XmlSerializer(typeof(List<Gestor>));
                var writer = new StreamWriter(xmlFilePath);
                xml.Serialize(writer, records);
                writer.Close();
                Console.WriteLine("Fitxer XML creat correctament");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error al convertir a XML: " + e.Message);
            }
        }

        public static List<Gestor> ReadXML(string xmlFilePath) //Llegir XML
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(List<Gestor>));
                using (var reader = new StreamReader(xmlFilePath))
                {
                    return (List<Gestor>)xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("S'ha produït un error en llegir el fitxer XML: " + ex.Message);
                return new List<Gestor>();
            }
        }

        public static void ComarquesPoblacio(List<Gestor> records) //Comarques amb població superior a 200.000 habitants
        {
            var comarquesSuperior200K = new List<Gestor>();

            foreach (var gestor in records)
            {
                if (gestor.Poblacio > 200000)
                {
                    comarquesSuperior200K.Add(gestor);
                }
            }
            Console.WriteLine("Comarques amb població superior a 200.000 habitants:");
            foreach (var gestor in comarquesSuperior200K)
            {
                Console.WriteLine($"Comarca: {gestor.Comarca}, Població: {gestor.Poblacio}");
            }

        }


        public static void CalcularMitjanaConsumDomesticPerComarca(List<Gestor> records)
        {
            var comarquesMitjana = records
                .GroupBy(r => r.Comarca) // Agrupar registros por comarca
                .Select(group => new Gestor
                {
                    Comarca = group.Key, // La clave del grupo es el nombre de la comarca
                    ConsDomCap = group.Average(r => r.ConsDomCap) // Calcular la media del consumo doméstico para el grupo
                })
                .ToList();

            Console.WriteLine("Mitjana de consum domèstic per comarca:");
            foreach (var gestor in comarquesMitjana)
            {
                Console.WriteLine($"Comarca: {gestor.Comarca}, Mitjana: {gestor.ConsDomCap}");
            }
        }



        public static void ComarquesConsumDomesticMesAlt(List<Gestor> records)
        {
            var comarquesConsumMesAlt = records
             .GroupBy(g => g.Comarca) // Agrupar por comarca
             .Select(g => new { Comarca = g.Key, MaxConsum = g.Max(x => x.ConsDomCap) }) // Seleccionar la comarca amb el máxim consum doméstic
             .OrderByDescending(x => x.MaxConsum); // Ordenar pel máxim consum doméstic de manera descendent

            Console.WriteLine("Comarques amb el consum domèstic per càpita més alt:");
            foreach (var item in comarquesConsumMesAlt)
            {
                Console.WriteLine($"Comarca: {item.Comarca}, Consum domèstic més alt: {item.MaxConsum}");
            }

        }

        public static void ComarquesConsumDomesticMesBaix(List<Gestor> records)
        {
            var comarquesConsumMesBaix = records
                .GroupBy(g => g.Comarca) // Agrupar por comarca
                .Select(g => new { Comarca = g.Key, MinConsum = g.Min(x => x.ConsDomCap) }) // Seleccionar la comarca con el minim consum doméstic
                .OrderBy(x => x.MinConsum); // Ordenar por el mínimo consumo doméstico de forma ascendent

            Console.WriteLine("Comarques amb el consum domèstic per càpita més baix:");
            foreach (var item in comarquesConsumMesBaix)
            {
                Console.WriteLine($"Comarca: {item.Comarca}, Consum domèstic més baix: {item.MinConsum}");
            }
        }

        public static void FiltrarYMostrarComarcas(List<Gestor> records, string filtro = "")
        {
            IEnumerable<Gestor> comarcasFiltradas = records;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                comarcasFiltradas = records.Where(g => g.Comarca.Contains(filtro) || g.CodiComarca.ToString() == filtro);
            }

            if (comarcasFiltradas.Any())
            {
                Console.WriteLine(string.IsNullOrWhiteSpace(filtro) ? "Totes les comarques:" : $"Comarques amb el filtre '{filtro}':");
                foreach (var comarca in comarcasFiltradas)
                {
                    Console.WriteLine($"Comarca: {comarca.Comarca}, Codi comarca: {comarca.CodiComarca}, Consum domèstic per càpita: {comarca.ConsDomCap}");
                }
            }
            else
            {
                Console.WriteLine($"No s'han trobat comarques amb el filtre '{filtro}'.");
            }
        }
    }
}
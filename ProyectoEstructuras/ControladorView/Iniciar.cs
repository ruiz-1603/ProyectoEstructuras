using BuscadorIndiceInvertido.ContoladorView;

namespace BuscadorIndiceInvertido.Interfaz
{
    public class IniciarSistema
    {
        public static void Iniciar()
        {
            MostrarBienvenida();

            Console.WriteLine("Iniciando Sistema de Búsqueda...");
            Console.WriteLine(new string('-', 50));

            // Paso 1: Procesar documentos
            Console.Write("Procesando documentos... ");
            if (!Controller.Iniciar())
            {
                Console.WriteLine("FALLÓ");
                Console.WriteLine("No se pudo inicializar el sistema.");
                EsperarTecla();
                return;
            }
            Console.WriteLine("OK");

            // Paso 2: Construir índice (incluye aplicación de Zipf)
            Console.Write("Construyendo índice invertido... ");
            if (!Controller.ConstruirIndice())
            {
                Console.WriteLine("FALLÓ");
                Console.WriteLine("No se pudo construir el índice.");
                EsperarTecla();
                return;
            }
            Console.WriteLine("OK");

            Console.WriteLine();
            Console.WriteLine("✓ Sistema inicializado correctamente");
            Console.WriteLine("✓ Listo para realizar búsquedas");
            Console.WriteLine();

            // Iniciar interfaz de búsqueda
            Controller.Buscar();
        }

        private static void MostrarBienvenida()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║        SISTEMA DE BÚSQUEDA POR ÍNDICE INVERTIDO      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        private static void EsperarTecla()
        {
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
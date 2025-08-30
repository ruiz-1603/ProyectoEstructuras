using BuscadorIndiceInvertido.ContoladorView;
using System;

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

            // Obtener percentil del usuario
            double percentile = ObtenerPercentilUsuario();

            // Paso 2: Construir índice
            Console.Write("Construyendo índice invertido... ");
            if (!Controller.ConstruirIndice(percentile))
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

        private static double ObtenerPercentilUsuario()
        {
            double percentile;
            while (true)
            {
                Console.WriteLine();
                Console.Write("Ingrese el percentil de palabras a eliminar (ej. 0.1 para 10%, rango 0.1-0.7): ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out percentile) && percentile >= 0.1 && percentile <= 0.7)
                {
                    return percentile;
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un número entre 0.1 y 0.7.");
                }
            }
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
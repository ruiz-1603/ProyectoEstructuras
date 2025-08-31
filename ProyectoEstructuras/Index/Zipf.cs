using BuscadorIndiceInvertido.Utilidades;
using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Ordenamientos;

namespace BuscadorIndiceInvertido.Index
{
    internal class Zipf
    {
        private double umbralPercentil;

        public Zipf()
        {
            umbralPercentil = SolicitarPercentil();
        }

        private double SolicitarPercentil()
        {
            double percentil;
            while (true)
            {
                Console.Write("Ingrese el percentil de palabras menos frecuentes a eliminar (0.0 - 0.7): ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out percentil))
                {
                    if (percentil >= 0.0 && percentil <= 0.7)
                    {
                        return percentil;
                    }
                }

                Console.WriteLine("Error: Ingrese un valor entre 0.0 y 0.7");
            }
        }

        public void AplicarLeyZipf(IndiceInvertido indice)
        {
            Console.Write($"Aplicando Ley de Zipf con percentil {umbralPercentil:F2}... ");

            if (umbralPercentil == 0.0)
            {
                Console.WriteLine("OK (sin filtrado)");
                return;
            }

            // 1. Obtener datos básicos del índice
            string[] vocabulario = indice.GetVocabulario();
            int totalPalabras = indice.GetPalabrasCount();

            if (totalPalabras == 0)
            {
                Console.WriteLine("OK (vocabulario vacío)");
                return;
            }

            // 2. Calcular frecuencias - optimizado
            int[] frecuencias = CalcularTodasFrecuencias(indice, vocabulario, totalPalabras);

            // 3. Encontrar umbral usando algoritmo ultra-simple y confiable
            int umbralFrecuencia = EncontrarUmbralSimple(frecuencias);

            // 4. Identificar y eliminar palabras de baja frecuencia
            EliminarPalabrasBajaFrecuencia(indice, vocabulario, frecuencias, umbralFrecuencia);

            Console.WriteLine("OK");
        }

        private int[] CalcularTodasFrecuencias(IndiceInvertido indice, string[] vocabulario, int totalPalabras)
        {
            int[] frecuencias = new int[totalPalabras];

            for (int i = 0; i < totalPalabras; i++)
            {
                var postings = indice.GetPostings(vocabulario[i]);
                int total = 0;
                foreach (var (doc, freq) in postings)
                {
                    total += freq;
                }
                frecuencias[i] = total;
            }

            return frecuencias;
        }

        // Algoritmo ultra-simple: ordenar directamente y tomar percentil
        private int EncontrarUmbralSimple(int[] frecuencias)
        {
            if (frecuencias.Length == 0) return 0;
            if (frecuencias.Length == 1) return frecuencias[0];

            // Crear copia y ordenar usando HeapSort (más estable que QuickSort)
            int[] copia = new int[frecuencias.Length];
            for (int i = 0; i < frecuencias.Length; i++)
            {
                copia[i] = frecuencias[i];
            }

            // Usar HeapSort - O(n log n) garantizado, sin riesgo de bucles infinitos
            HeapSortDescendente(copia);

            // Calcular índice del percentil
            int indiceUmbral = (int)(copia.Length * (1.0 - umbralPercentil));
            if (indiceUmbral >= copia.Length) indiceUmbral = copia.Length - 1;
            if (indiceUmbral < 0) indiceUmbral = 0;

            return copia[indiceUmbral];
        }

        private void EliminarPalabrasBajaFrecuencia(IndiceInvertido indice, string[] vocabulario,
                                                   int[] frecuencias, int umbralFrecuencia)
        {
            var palabrasAEliminar = new DoubleList<string>();

            for (int i = 0; i < vocabulario.Length; i++)
            {
                if (frecuencias[i] < umbralFrecuencia)
                {
                    palabrasAEliminar.Add(vocabulario[i]);
                }
            }

            if (palabrasAEliminar.Count > 0)
            {
                indice.EliminarPalabras(palabrasAEliminar);
            }
        }

        // HeapSort - algoritmo más estable que QuickSort, sin riesgo de bucles infinitos
        private void HeapSortDescendente(int[] arr)
        {
            int n = arr.Length;

            // Construir max-heap
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(arr, n, i);
            }

            // Extraer elementos del heap uno por uno
            for (int i = n - 1; i > 0; i--)
            {
                // Mover la raíz actual al final
                Intercambiar(arr, 0, i);

                // Llamar heapify en el heap reducido
                Heapify(arr, i, 0);
            }
        }

        private void Heapify(int[] arr, int n, int i)
        {
            int mayor = i;
            int izq = 2 * i + 1;
            int der = 2 * i + 2;

            // Si el hijo izquierdo es mayor que la raíz
            if (izq < n && arr[izq] > arr[mayor])
                mayor = izq;

            // Si el hijo derecho es mayor que el mayor hasta ahora
            if (der < n && arr[der] > arr[mayor])
                mayor = der;

            // Si el mayor no es la raíz
            if (mayor != i)
            {
                Intercambiar(arr, i, mayor);

                // Recursivamente heapify el subárbol afectado
                Heapify(arr, n, mayor);
            }
        }

        private void Intercambiar(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}
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

            // 2. Calcular frecuencias de forma más eficiente
            int[] frecuencias = new int[totalPalabras];
            for (int i = 0; i < totalPalabras; i++)
            {
                var postings = indice.GetPostings(vocabulario[i]);
                frecuencias[i] = CalcularFrecuenciaTotal(postings);
            }

            // 3. Encontrar umbral usando algoritmo más eficiente
            int umbralFrecuencia = EncontrarUmbralRapido(frecuencias);

            // 4. Identificar palabras a eliminar sin ordenar
            var palabrasAEliminar = new DoubleList<string>();
            for (int i = 0; i < totalPalabras; i++)
            {
                if (frecuencias[i] < umbralFrecuencia)
                {
                    palabrasAEliminar.Add(vocabulario[i]);
                }
            }

            // 5. Eliminar palabras del índice
            if (palabrasAEliminar.Count > 0)
            {
                indice.EliminarPalabras(palabrasAEliminar);
            }

            Console.WriteLine("OK");
        }

        private int CalcularFrecuenciaTotal(DoubleList<(Doc doc, int freq)> postings)
        {
            int total = 0;
            foreach (var (doc, freq) in postings)
            {
                total += freq;
            }
            return total;
        }

        // Algoritmo de selección rápida para encontrar el percentil sin ordenar todo
        private int EncontrarUmbralRapido(int[] frecuencias)
        {
            if (frecuencias.Length == 0) return 0;

            // Crear copia para no modificar el original
            int[] copia = new int[frecuencias.Length];
            for (int i = 0; i < frecuencias.Length; i++)
            {
                copia[i] = frecuencias[i];
            }

            // Calcular el índice del percentil
            int indiceObjetivo = (int)(copia.Length * (1.0 - umbralPercentil));
            if (indiceObjetivo >= copia.Length) indiceObjetivo = copia.Length - 1;
            if (indiceObjetivo < 0) indiceObjetivo = 0;

            // Usar QuickSelect para encontrar el k-ésimo elemento más grande
            return QuickSelect(copia, 0, copia.Length - 1, indiceObjetivo);
        }

        // QuickSelect optimizado - O(n) en promedio
        private int QuickSelect(int[] arr, int inicio, int fin, int k)
        {
            if (inicio == fin) return arr[inicio];

            // Partición simple
            int pivotIndex = ParticionSimple(arr, inicio, fin);

            if (k == pivotIndex)
                return arr[k];
            else if (k < pivotIndex)
                return QuickSelect(arr, inicio, pivotIndex - 1, k);
            else
                return QuickSelect(arr, pivotIndex + 1, fin, k);
        }

        private int ParticionSimple(int[] arr, int inicio, int fin)
        {
            // Usar mediana de tres para mejor pivot
            int medio = inicio + (fin - inicio) / 2;
            if (arr[medio] > arr[fin]) Intercambiar(arr, medio, fin);
            if (arr[inicio] > arr[fin]) Intercambiar(arr, inicio, fin);
            if (arr[medio] > arr[inicio]) Intercambiar(arr, medio, inicio);

            int pivot = arr[inicio];
            int i = inicio + 1;
            int j = fin;

            while (true)
            {
                while (i <= j && arr[i] >= pivot) i++; // Orden descendente
                while (i <= j && arr[j] < pivot) j--;

                if (i > j) break;

                Intercambiar(arr, i, j);
                i++;
                j--;
            }

            Intercambiar(arr, inicio, j);
            return j;
        }

        private void Intercambiar(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}
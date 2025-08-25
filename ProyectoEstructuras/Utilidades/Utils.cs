using BuscadorIndiceInvertido.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscadorIndiceInvertido.Ordenamientos
{
    internal static class Utils
    {
        public static void OrdenarAlfab(string[] arr, int ini, int final)
        {
            // ordenar en alfabetico utilizando QuickSort
            if (ini < final)
            {
                int pivot = Particion(arr, ini, final);

                OrdenarAlfab(arr, ini, pivot - 1);
                OrdenarAlfab(arr, pivot + 1, final);
            }
        }

        private static int Particion(string[] arr, int ini, int final)
        {
            string pivot = arr[final];
            int i = ini - 1;

            for (int j = ini; j < final; j++)
            {
                // si arr[j] va primero entonces devuelve un numero negativo
                if (string.Compare(arr[j], pivot, StringComparison.Ordinal) <= 0)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }

            (arr[i + 1], arr[final]) = (arr[final], arr[i + 1]);
            return i + 1;
        }

        public static int BusquedaBinaria(string palabra, string[] arr)
        {
            int ini = 0;
            int final = arr.Length - 1;

            while (ini <= final)
            {
                int mid = ini + (final - ini) / 2;
                int result = string.Compare(palabra, arr[mid], StringComparison.Ordinal);

                if (result == 0)
                    return mid;
                else if (result < 0)
                    final = mid - 1;
                else
                    ini = mid + 1;
            }

            return -1;
        }

        public static void OrdenarPorPuntaje((Doc doc, double score)[] arr, int ini, int final)
        {
            if (ini < final)
            {
                int pivot = ParticionarPorScore(arr, ini, final);
                OrdenarPorPuntaje(arr, ini, pivot - 1);
                OrdenarPorPuntaje(arr, pivot + 1, final);
            }
        }

        private static int ParticionarPorScore((Doc doc, double score)[] arr, int ini, int final)
        {
            double pivotScore = arr[final].score;
            int i = ini - 1;

            for (int j = ini; j < final; j++)
            {
                if (arr[j].score >= pivotScore) // Orden descendente
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }

            (arr[i + 1], arr[final]) = (arr[final], arr[i + 1]);
            return i + 1;
        }
    }
}

using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Estrategias;
using System;

namespace BuscadorIndiceInvertido.Ordenamientos
{
    internal static class Utils
    {
        private static IEstrategiaOrdenamiento estrategia = new QuickSortStrategy();

        // Método para cambiar la estrategia
        public static void CambiarEstrategia(IEstrategiaOrdenamiento nuevaEstrategia)
        {
            estrategia = nuevaEstrategia;
        }

        // Métodos delegados que mantienen la misma interfaz pública
        public static void OrdenarAlfab(string[] arr, int ini, int final)
        {
            estrategia.OrdenarAlfabetico(arr, ini, final);
        }

        public static void OrdenarPorPuntaje((Doc doc, double score)[] arr, int ini, int final)
        {
            estrategia.OrdenarPorPuntaje(arr, ini, final);
        }

        public static int BusquedaBinaria(string palabra, string[] arr)
        {
            return estrategia.BusquedaBinaria(palabra, arr);
        }

        // Métodos de conveniencia para cambiar estrategias específicas
        public static void UsarQuickSort()
        {
            estrategia = new QuickSortStrategy();
        }

        public static void UsarMergeSort()
        {
            estrategia = new MergeSortStrategy();
        }
    }
}
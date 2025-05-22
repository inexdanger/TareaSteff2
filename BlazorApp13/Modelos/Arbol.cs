using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp13.Modelos
{
    public class Nodo
    {
        public int Valor;
        public Nodo? Izquierdo;
        public Nodo? Derecho;

        public Nodo(int valor)
        {
            Valor = valor;
            Izquierdo = null;
            Derecho = null;
        }
    }

    public class NodoD3
    {
        public int name { get; set; }
        public List<NodoD3>? children { get; set; }
    }

    public class Arbol
    {
        public Nodo? Raiz;

        private readonly int[] valoresPredeterminados = new int[]
        {
            50, 30, 70, 20, 10, 5, 25, 60, 80, 90,
            85, 65, 35, 33, 34, 32, 95, 72, 73, 74,
            36, 15, 55, 62, 67, 68, 28, 8, 1, 88
        };

        public Arbol()
        {
            Raiz = null;
            foreach (var v in valoresPredeterminados)
            {
                Raiz = Insertar(Raiz, v);
            }
        }

        // Inserción
        public Nodo Insertar(Nodo? nodo, int valor)
        {
            if (nodo == null) return new Nodo(valor);

            if (valor < nodo.Valor)
                nodo.Izquierdo = Insertar(nodo.Izquierdo, valor);
            else if (valor > nodo.Valor)
                nodo.Derecho = Insertar(nodo.Derecho, valor);

            return nodo;
        }

        public void InsertarPublico(int valor)
        {
            Raiz = Insertar(Raiz, valor);
        }

        public async Task InsertarAnimado(int valor, Func<Nodo?, Task> onVisit)
        {
            Nodo? actual = Raiz;
            Nodo? padre = null;
            while (actual != null)
            {
                await onVisit(actual);
                await Task.Delay(500);
                padre = actual;
                if (valor < actual.Valor)
                    actual = actual.Izquierdo;
                else if (valor > actual.Valor)
                    actual = actual.Derecho;
                else
                    return; // Ya existe
            }

            Nodo nuevo = new Nodo(valor);
            if (padre == null)
                Raiz = nuevo;
            else if (valor < padre.Valor)
                padre.Izquierdo = nuevo;
            else
                padre.Derecho = nuevo;

            await onVisit(nuevo);
        }

        // Búsqueda
        public bool Buscar(int valor)
        {
            return BuscarRecursivo(Raiz, valor);
        }

        private bool BuscarRecursivo(Nodo? nodo, int valor)
        {
            if (nodo == null) return false;
            if (valor == nodo.Valor) return true;
            if (valor < nodo.Valor)
                return BuscarRecursivo(nodo.Izquierdo, valor);
            else
                return BuscarRecursivo(nodo.Derecho, valor);
        }

        public async Task<bool> BuscarAnimado(int valor, Func<Nodo?, Task> onVisit)
        {
            Nodo? actual = Raiz;
            while (actual != null)
            {
                await onVisit(actual);
                await Task.Delay(500);
                if (valor == actual.Valor)
                    return true;

                actual = valor < actual.Valor ? actual.Izquierdo : actual.Derecho;
            }

            await onVisit(null);
            return false;
        }
        // Eliminación
       private Nodo? Eliminar(Nodo? nodo, int valor)
        {
            if (nodo == null) return null;

            if (valor < nodo.Valor)
                nodo.Izquierdo = Eliminar(nodo.Izquierdo, valor);
            else if (valor > nodo.Valor)
                nodo.Derecho = Eliminar(nodo.Derecho, valor);
            else
            {
                // Nodo encontrado
                if (nodo.Izquierdo == null)
                    return nodo.Derecho;
                else if (nodo.Derecho == null)
                    return nodo.Izquierdo;

                Nodo sucesor = MinimoValor(nodo.Derecho);
                nodo.Valor = sucesor.Valor;
                nodo.Derecho = Eliminar(nodo.Derecho, sucesor.Valor);
            }
            return nodo;
        }

        public void EliminarPublico(int valor)
        {
            Raiz = Eliminar(Raiz, valor);
        }

        public async Task EliminarAnimado(int valor, Func<Nodo?, Task> onVisit)
        {
            Raiz = await EliminarAnimado(Raiz, valor, onVisit);
        }

        private async Task<Nodo?> EliminarAnimado(Nodo? nodo, int valor, Func<Nodo?, Task> onVisit)
        {
            if (nodo == null)
            {
                await onVisit(null);
                return null;
            }

            await onVisit(nodo);
            await Task.Delay(500);

            if (valor < nodo.Valor)
                nodo.Izquierdo = await EliminarAnimado(nodo.Izquierdo, valor, onVisit);
            else if (valor > nodo.Valor)
                nodo.Derecho = await EliminarAnimado(nodo.Derecho, valor, onVisit);
            else
            {
                if (nodo.Izquierdo == null)
                    return nodo.Derecho;
                else if (nodo.Derecho == null)
                    return nodo.Izquierdo;

                Nodo sucesor = MinimoValor(nodo.Derecho);
                nodo.Valor = sucesor.Valor;
                nodo.Derecho = await EliminarAnimado(nodo.Derecho, sucesor.Valor, onVisit);
            }
            return nodo;
        }

        private Nodo MinimoValor(Nodo nodo)
        {
            Nodo actual = nodo;
            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }
            return actual;
        }

        // Recorridos
        public List<int> ObtenerRecorridoInorden()
        {
            var lista = new List<int>();
            RecorrerInorden(Raiz, lista);
            return lista;
        }

        private void RecorrerInorden(Nodo? nodo, List<int> lista)
        {
            if (nodo == null) return;
            RecorrerInorden(nodo.Izquierdo, lista);
            lista.Add(nodo.Valor);
            RecorrerInorden(nodo.Derecho, lista);
        }

        public List<int> ObtenerRecorridoPreorden()
        {
            var lista = new List<int>();
            RecorrerPreorden(Raiz, lista);
            return lista;
        }

        private void RecorrerPreorden(Nodo? nodo, List<int> lista)
        {
            if (nodo == null) return;
            lista.Add(nodo.Valor);
            RecorrerPreorden(nodo.Izquierdo, lista);
            RecorrerPreorden(nodo.Derecho, lista);
        }

        public List<int> ObtenerRecorridoPostorden()
        {
            var lista = new List<int>();
            RecorrerPostorden(Raiz, lista);
            return lista;
        }

        private void RecorrerPostorden(Nodo? nodo, List<int> lista)
        {
            if (nodo == null) return;
            RecorrerPostorden(nodo.Izquierdo, lista);
            RecorrerPostorden(nodo.Derecho, lista);
            lista.Add(nodo.Valor);
        }
        public async Task RecorrerPreordenAnimado(Func<Nodo?, Task> onVisit)
        {
            await RecorrerPreordenAnimadoInterno(Raiz, onVisit);
        }

        private async Task RecorrerPreordenAnimadoInterno(Nodo? nodo, Func<Nodo?, Task> onVisit)
        {
            if (nodo == null) return;

            await onVisit(nodo);
            await Task.Delay(200);

            await RecorrerPreordenAnimadoInterno(nodo.Izquierdo, onVisit);
            await RecorrerPreordenAnimadoInterno(nodo.Derecho, onVisit);
        }

        public async Task RecorrerInordenAnimado(Func<Nodo?, Task> onVisit)
        {
            await RecorrerInordenAnimadoInterno(Raiz, onVisit);
        }

        private async Task RecorrerInordenAnimadoInterno(Nodo? nodo, Func<Nodo?, Task> onVisit)
        {
            if (nodo == null) return;

            await RecorrerInordenAnimadoInterno(nodo.Izquierdo, onVisit);

            await onVisit(nodo);
            await Task.Delay(200);

            await RecorrerInordenAnimadoInterno(nodo.Derecho, onVisit);
        }

        public async Task RecorrerPostordenAnimado(Func<Nodo?, Task> onVisit)
        {
            await RecorrerPostordenAnimadoInterno(Raiz, onVisit);
        }

        private async Task RecorrerPostordenAnimadoInterno(Nodo? nodo, Func<Nodo?, Task> onVisit)
        {
            if (nodo == null) return;

            await RecorrerPostordenAnimadoInterno(nodo.Izquierdo, onVisit);
            await RecorrerPostordenAnimadoInterno(nodo.Derecho, onVisit);

            await onVisit(nodo);
            await Task.Delay(200);
        }


        public string ImprimirArbol()
        {
            var valores = ObtenerRecorridoInorden();
            return string.Join(", ", valores);
        }

        public NodoD3? ConvertirANodoD3(Nodo? nodo)
        {
            if (nodo == null) return null;

            var nd3 = new NodoD3
            {
                name = nodo.Valor,
                children = new List<NodoD3>()
            };

            if (nodo.Izquierdo != null)
                nd3.children.Add(ConvertirANodoD3(nodo.Izquierdo)!);

            if (nodo.Derecho != null)
                nd3.children.Add(ConvertirANodoD3(nodo.Derecho)!);

            if (nd3.children.Count == 0)
                nd3.children = null;

            return nd3;
        }


        public NodoD3? ObtenerArbolParaD3()
        {
            return ConvertirANodoD3(Raiz);
        }

        public async Task RecorrerHastaNodo(int valor, Func<Nodo?, Task> onVisit)
        {
            Nodo? actual = Raiz;

            while (actual != null)
            {
                await onVisit(actual);
                await Task.Delay(500);

                if (valor == actual.Valor)
                    return;

                actual = valor < actual.Valor ? actual.Izquierdo : actual.Derecho;
            }

            await onVisit(null);
        }
    }
}

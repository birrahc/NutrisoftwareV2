using Microsoft.Extensions.Caching.Memory;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Svc
{
    public class SvcMemoryCache
    {
       

        public static List<T> ListarEntidade<T>(IMemoryCache memory, string chave)
        {
            return memory.TryGetValue(chave, out List<T> alimentos) ?
                alimentos : new List<T>();
        }

        public static T BuscarEntidade<T>(IMemoryCache memory, string chave) where T : new()
        {
            return memory.TryGetValue(chave, out T Entidade) ?
                Entidade : new T();
        }
        public static void AmarzenarEntidade<T>(T pEntidade, IMemoryCache memory, string chave)
        {
            List<T> Entidades = (List<T>)ListarEntidade<T>(memory, chave) !=null ?
                (List<T>)ListarEntidade<T>(memory, chave):new List<T>();
            Entidades.Add(pEntidade);
            memory.Set(chave, Entidades);
        }
        public static void AmarzenarEntidadeUnica<T>(T pEntidade, IMemoryCache memory, string chave) where T:new()
        {
            T Entidades = (T)pEntidade != null ?
                (T)pEntidade : new T();
           
            memory.Set(chave, Entidades);
        }

        public static void RemoverItemEntidade<T>(T pEntidade, IMemoryCache memory, string chave) 
        {
            ListarEntidade<T>(memory, chave).Remove(pEntidade);
        }


        public static void ArmazenarRangeEntidade<T>(List<T> pEntidades, IMemoryCache memory, string chave)
        {
            List<T> Entidades = ListarEntidade<T>(memory, chave) != null ?
                ListarEntidade<T>(memory, chave) : new List<T>();
            Entidades.AddRange(pEntidades);
            memory.Set(chave, Entidades);

        }
    }
}

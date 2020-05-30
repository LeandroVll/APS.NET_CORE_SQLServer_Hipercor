using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HipercoreASPNETCORE.Models;

namespace HipercoreASPNETCORE.Interfaces
{
    public interface IServicioEnvioEmail
    {
        Boolean EnvioDeEmail(string Destino, string Asunto, string CuerpoHTML);
    }
}

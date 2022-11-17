// Fernando Hernández Domínguez
using System;

namespace Generador{
    public class Error : Exception{
        public Error(String mensaje, StreamWriter log) : base(mensaje) {
            log.WriteLine(mensaje);
        }
    }
}
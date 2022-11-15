// Fernando Hernández Domínguez
using System;
using System.IO;

namespace Generador{
    public class Error : Exception{
        public Error(String mensaje, StreamWriter log) : base(mensaje) {
            log.WriteLine(mensaje);
        }
    }
}
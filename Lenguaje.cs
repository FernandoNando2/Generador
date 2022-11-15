// Fernando Hernández Domínguez
using System;
using System.Collections.Generic;

namespace Generador{
    class Lenguaje : Sintaxis, IDisposable{

        public void Dispose(){
            cerrar();
        }
        
    }
}
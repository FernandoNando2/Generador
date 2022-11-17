// Fernando Hernández Domínguez
/*
Requerimiento 1: Construir un metodo para escribir en el archivo lenguaje.cs para indentar el codigo
                    { incrementa un tab
                    } decrementa un tab
Requerimiento 2: Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la primera produccion 
                 de la gramatica
Requerimiento 3: La primera prosuccion es publica, las demas son privadas
*/
using System;

namespace Generador{
    class Lenguaje : Sintaxis, IDisposable{
        string nombreProyecto;
        public Lenguaje(String ruta) : base(ruta) { nombreProyecto = ""; }

        public Lenguaje(){ nombreProyecto = ""; }

        public void Dispose(){
            cerrar();
        }

        private void Programa(string espacioProyecto, string metodo){
            programa.WriteLine("using System;");
            programa.WriteLine("namespace "+espacioProyecto +"{");
            programa.WriteLine("\tpublic class Program{");
            programa.WriteLine("\t\tstatic void Main(string[] args){");
            programa.WriteLine("\t\t\tusing(Lenguaje a = new Lenguaje()){");
            programa.WriteLine("\t\t\t\ttry{");
            programa.WriteLine("\t\t\t\t\ta."+metodo+"();");
            programa.WriteLine("\t\t\t\t}");
            programa.WriteLine("\t\t\t\tcatch(Exception e){");
            programa.WriteLine("\t\t\t\t\tConsole.WriteLine(e.Message);");
            programa.WriteLine("\t\t\t\t}");
            programa.WriteLine("\t\t\t}");
            programa.WriteLine("\t\t}");
            programa.WriteLine("\t}");
            programa.WriteLine("}");
        }

        private void cabeceraLenguaje(string espacioProyecto){
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("namespace " +nombreProyecto +"{");
            lenguaje.WriteLine("\tclass Lenguaje : Sintaxis, IDisposable{");
            lenguaje.WriteLine("\t\tstring nombreProyecto;");
            lenguaje.WriteLine("\t\tpublic Lenguaje(String ruta) : base(ruta) { nombreProyecto = \"\"; }");
            lenguaje.WriteLine("\t\tpublic Lenguaje(){ nombreProyecto = \"\"; }");
            lenguaje.WriteLine("\t\tpublic void Dispose(){");
            lenguaje.WriteLine("\t\t\tcerrar();");
            lenguaje.WriteLine("\t\t}");
        }
        public void gramatica(){
            cabecera();
            Programa(nombreProyecto,"programa");
            cabeceraLenguaje(nombreProyecto);
            listaProducciones();
            lenguaje.WriteLine("\t}");
            lenguaje.WriteLine("}");
        }

        public void cabecera(){
            match("Gramatica");
            match(":");
            nombreProyecto = getContenido();
            match(tipos.snt);
            match(tipos.finProduccion);
        }

        public void listaProducciones(){
            lenguaje.WriteLine("\t\tprivate void " +getContenido() +"(){");
            match(tipos.snt);
            match(tipos.produce);
            match(tipos.finProduccion);
            lenguaje.WriteLine("\t\t}");
            if(!FinArchivo())
                listaProducciones();
        }
    }
}
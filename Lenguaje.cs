// Fernando Hernández Domínguez
/*
Requerimiento 1: Construir un metodo para escribir en el archivo lenguaje.cs para indentar el codigo
                    { incrementa un tab
                    } decrementa un tab
Requerimiento 2: Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la primera produccion 
                 de la gramatica
Requerimiento 3: La primera produccion es publica, las demas son privadas
Requerimiento 4: El constructor Lexico() parametrizado debe valiar que la extension del archivo a compilar sea .gen
                 si no es .gen debe lanzar una excepcion
*/
using System;

namespace Generador{
    class Lenguaje : Sintaxis, IDisposable{
        public Lenguaje(String ruta) : base(ruta) { }

        public Lenguaje(){ }

        public void Dispose(){
            cerrar();
        }

        private void Programa(string metodo){
            programa.WriteLine("using System;");
            programa.WriteLine("namespace Generico{");
            programa.WriteLine("\tpublic class Program{");
            programa.WriteLine("\t\tstatic void Main(string[] args){");
            programa.WriteLine("\t\t\tusing(Lenguaje a = new Lenguaje()){");
            programa.WriteLine("\t\t\t\ttry{");
            programa.WriteLine("\t\t\t\t\ta." +metodo +"();");
            programa.WriteLine("\t\t\t\t}");
            programa.WriteLine("\t\t\t\tcatch(Exception e){");
            programa.WriteLine("\t\t\t\t\tConsole.WriteLine(e.Message);");
            programa.WriteLine("\t\t\t\t}");
            programa.WriteLine("\t\t\t}");
            programa.WriteLine("\t\t}");
            programa.WriteLine("\t}");
            programa.WriteLine("}");
        }

        private void cabeceraLenguaje(){
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("namespace Generico{");
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
            Programa("programa");
            cabeceraLenguaje();
            listaProducciones();
            lenguaje.WriteLine("\t}");
            lenguaje.WriteLine("}");
        }

        public void cabecera(){
            match("Gramatica");
            match(":");
            match(tipos.snt);
            match(tipos.finProduccion);
        }

        public void listaProducciones(){
            lenguaje.WriteLine("\t\tprivate void " +getContenido() +"(){");
            match(tipos.snt);
            match(tipos.produce);
            simbolos();
            match(tipos.finProduccion);
            lenguaje.WriteLine("\t\t}");
            if(!FinArchivo())
                listaProducciones();
        }

        private void simbolos(){
            if(esTipo(getContenido())){
                lenguaje.WriteLine("\t\t\tmatch(tipos." +getContenido() +");");
                match(tipos.snt);
            }
            else if(getClasificacion() == tipos.st){
                lenguaje.WriteLine("\t\t\tmatch(\"" +getContenido() +"\");");
                match(tipos.st);
            }
            else if(getClasificacion() == tipos.finProduccion){
                
            }
            if(getClasificacion() != tipos.finProduccion)
                simbolos();
        }

        private bool esTipo(string clasificacion){
            switch(clasificacion){
                case "identificador":
                case "numero":
                case "caracter":
                case "asignacion":
                case "inicializacion":
                case "operadorLogico":
                case "operadorRelacional":
                case "operadorTernario":
                case "operadorTermino":
                case "operadorFactor":
                case "incrementoTermino":
                case "incrementoFactor":
                case "finSentencia":
                case "cadena":
                case "tipoDatos":
                case "zona":
                case "condicion":
                case "ciclo":
                    return true;
            }
            return false;
        }
    }
}
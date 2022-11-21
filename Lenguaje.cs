// Fernando Hernández Domínguez
/*
Requerimiento 1: Construir un metodo para escribir en el archivo lenguaje.cs para indentar el codigo ya
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
        int contador;
        public Lenguaje(String ruta) : base(ruta) { }

        public Lenguaje(){ }

        public void Dispose(){
            cerrar();
        }

        public void imprimir(string contenido, StreamWriter archivo){
            if(contenido.Equals("}")){
                if(contenido.Contains("{"))
                    contador++;
                if(contenido.Contains("}"))
                    contador--;
                for(int i = 0; i < contador; i++)
                    archivo.Write("    ");
            }
            else{
                for(int i = 0; i < contador; i++)
                    archivo.Write("    ");
                if(contenido.Contains("{"))
                    contador++;
                if(contenido.Contains("}"))
                    contador--;
            }
            archivo.WriteLine(contenido);
        }

        private void Programa(string metodo){
            contador = 0;
            imprimir("using System;", programa);
            imprimir("namespace Generico{", programa);
            imprimir("public class Programa{", programa);
            imprimir("static void Main(){", programa);
            imprimir("using(Lenguaje a = new Lenguaje()){", programa);
            imprimir("try{", programa);
            imprimir("a."+metodo+"();", programa);
            imprimir("}", programa);
            imprimir("catch(Exception e){", programa);
            imprimir("Console.WriteLine(e.Message);", programa);
            imprimir("}", programa);
            imprimir("}", programa);
            imprimir("}", programa);
            imprimir("}", programa);
        }

        private void cabeceraLenguaje(){
            contador = 0;
            imprimir("using System;", lenguaje);
            imprimir("namespace Generico{", lenguaje);
            imprimir("class Lenguaje : Sintaxis, IDisposable{", lenguaje);
            imprimir("public Lenguaje(String ruta) : base(ruta){}", lenguaje);
            imprimir("public Lenguaje(){}", lenguaje);
            imprimir("public void Dispose(){", lenguaje);
            imprimir("cerrar();", lenguaje);
            imprimir("}", lenguaje);
        }
        public void gramatica(){
            cabecera();
            Programa("programa");
            cabeceraLenguaje();
            listaProducciones();
            imprimir("}", lenguaje);
            imprimir("}", lenguaje);
            imprimir("}", programa);
        }

        public void cabecera(){
            match("Gramatica");
            match(":");
            match(tipos.snt);
            match(tipos.finProduccion);
        }

        public void listaProducciones(){
            imprimir("private void " +getContenido() +"(){", lenguaje);
            match(tipos.snt);
            match(tipos.produce);
            simbolos();
            match(tipos.finProduccion);
            imprimir("}", lenguaje);
            if(!FinArchivo())
                listaProducciones();
        }

        private void simbolos(){
            if(esTipo(getContenido())){
                imprimir("match(tipos." +getContenido() +");", lenguaje);
                match(tipos.snt);
            }
            else if(getClasificacion() == tipos.st){
                imprimir("match(\"" +getContenido() +"\");", lenguaje);
                match(tipos.st);
            }
            else if(getClasificacion() == tipos.snt){
                imprimir(getContenido() +"();", lenguaje);
                match(tipos.snt);
            }
            else
                throw new Exception("Error de sintaxis en la produccion " +getContenido());
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
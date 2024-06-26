// Fernando Hernández Domínguez
/*
Requerimiento 1: Construir un metodo para escribir en el archivo lenguaje.cs para indentar el codigo YA
                    { incrementa un tab
                    } decrementa un tab
Requerimiento 2: Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la primera produccion de la gramatica YA
Requerimiento 3: La primera produccion es publica, las demas son privadas YA
Requerimiento 4: El constructor Lexico() parametrizado debe valiar que la extension del archivo a compilar sea .gram si no es .gram debe lanzar 
                 una excepcion YA
Requerimiento 5: Resolver la ambigüedad de ST y SNT YA
                 Recorrer linea por linea el archivo gram para extraer el nombre de cada produccion
Requerimiento 6: Agregar el parentesis derecho e izquierdo escapados en la matriz de transiciones YA
Requerimiento 7: Implementar la cerradura epsilon YA
*/
using System;

namespace Generador{
    class Lenguaje : Sintaxis, IDisposable{
        int contador;
        long pos;
        string primeraProduccion;
        List<string> listaSNT;
        public Lenguaje(String ruta) : base(ruta) { primeraProduccion = ""; listaSNT = new List<string>(); }

        public Lenguaje(){ primeraProduccion = ""; listaSNT = new List<string>(); }

        public void Dispose(){
            cerrar();
        }

        private bool esSNT(string contenido){
            //return true;
            return listaSNT.Contains(contenido);
        }

        private void agregarSNT(){
            listaSNT.Add(getContenido());
            archivo.ReadLine();
            NextToken();
            if(!FinArchivo())
                agregarSNT();
            else{
                archivo.DiscardBufferedData();
                archivo.BaseStream.Seek(pos, SeekOrigin.Begin);
                NextToken();
            }
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
            imprimir("public class " +metodo +"{", programa);
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
            primeraProduccion = getContenido();
            Programa(primeraProduccion);
            cabeceraLenguaje();
            string var = getContenido();
            pos = posicion - var.Length;
            agregarSNT();
            listaProducciones();
            imprimir("}", lenguaje);
            imprimir("}", lenguaje);
            imprimir("}", programa);
        }

        public void cabecera(){
            match("Gramatica");
            match(":");
            match(tipos.st);
            match(tipos.finProduccion);
        }

        public void listaProducciones(){
            if(getContenido() == primeraProduccion)
                imprimir("public void " + getContenido() + "(){", lenguaje);
            else
                imprimir("private void " +getContenido() +"(){", lenguaje);
            match(tipos.st);
            match(tipos.produce);
            simbolos();
            match(tipos.finProduccion);
            imprimir("}", lenguaje);
            if(!FinArchivo())
                listaProducciones();
        }

        private void simbolos(){
            if(getClasificacion() == tipos.pIzq){
                match(tipos.pIzq);
                if(esTipo(getContenido()))
                    imprimir("if(getClasificacion() == tipos." +getContenido() +"){", lenguaje);
                else
                    imprimir("if(getContenido() == \"" +getContenido() +"\"){", lenguaje);
                simbolos();
                match(tipos.pDer);
                imprimir("}", lenguaje);
            }
            else if(esTipo(getContenido())){
                imprimir("match(tipos." +getContenido() +");", lenguaje);
                match(tipos.st);
            }
            else if(esSNT(getContenido())){
                imprimir(getContenido() +"();", lenguaje);
                match(tipos.st);
            }
            else if(getClasificacion() == tipos.st){
                imprimir("match(\"" +getContenido() +"\");", lenguaje);
                match(tipos.st);
            }
            else
                throw new Exception("Error de sintaxis en la produccion " +getContenido());
            if(getClasificacion() != tipos.finProduccion && getClasificacion() != tipos.pDer)
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
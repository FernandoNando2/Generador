// Fernando Hernandez Dominguez
using System;
namespace Generador{
    public class Lexico : Token{
        protected StreamReader archivo;
        protected StreamWriter log;
        protected StreamWriter lenguaje;
        protected StreamWriter programa;
        const int F = -1;
        const int E = -2;
        protected int linea, posicion;
        int[,] TRAND = new int[,]{
            { 0, 1, 5, 3, 4, 5},
            { F, F, 2, F, F, F},
            { F, F, F, F, F, F},
            { F, F, F, 3, F, F},
            { F, F, F, F, F, F},
            { F, F, F, F, F, F},
        };
        public Lexico(){
            linea = 1;
            string path = "C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Generador\\prueba.cpp";
            bool existencia = File.Exists(path);
            log = new StreamWriter("C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Generador\\c.gram"); 
            log.AutoFlush = true;
            lenguaje = new StreamWriter("C:\\Generador\\Lenguaje.cs");
            lenguaje.AutoFlush = true;
            programa = new StreamWriter("C:\\Generador\\Program.cs");
            programa.AutoFlush = true;
            log.WriteLine("Archivo: cgram");
            log.WriteLine("Fecha: "+DateTime.Now);
            if (existencia == true)
                archivo = new StreamReader(path);
            else
                throw new Error("Error: El archivo c.gram no existe", log);
        }
        public Lexico(string nombre){
            linea = 1;
            string pathlog = Path.ChangeExtension(nombre, ".log");
            log = new StreamWriter(pathlog); 
            log.AutoFlush = true;
            lenguaje = new StreamWriter("C:\\Generador\\Lenguaje.cs");
            lenguaje.AutoFlush = true;
            programa = new StreamWriter("C:\\Generador\\Program.cs");
            programa.AutoFlush = true;
            log.WriteLine("Archivo: "+nombre);
            log.WriteLine("Fecha: "+DateTime.Now);
            if (File.Exists(nombre))
                archivo = new StreamReader(nombre);
            else
                throw new Error("Error: El archivo " +Path.GetFileName(nombre)+ " no existe ", log);
        }
        public void cerrar(){
            archivo.Close();
            log.Close();
            lenguaje.Close();
            programa.Close();
        }       
        private void clasifica(int estado){
            switch(estado){
                case 1:
                    setClasificacion(tipos.st);
                    break;
                case 2:
                    setClasificacion(tipos.produce);
                    break;
                case 3:
                    setClasificacion(tipos.st);
                    break;
                case 4:
                    setClasificacion(tipos.finProduccion);
                    break;
                case 5:
                    setClasificacion(tipos.st);
                    break;
            }
        }
        private int columna(char c){
            if (c == 10)
                return 4;
            else if (char.IsWhiteSpace(c))
                return 0;
            else if (c == '-')
                return 1;
            else if (c == '>')
                return 2;
            else if (char.IsLetter(c))
                return 3;
            return 5;
        }
        public void NextToken(){
            string buffer = "";
            char c;
            int estado = 0;
            while(estado >= 0){
                c = (char)archivo.Peek(); //Funcion de transicion
                estado = TRAND[estado,columna(c)];
                clasifica(estado);
                if (estado >= 0){
                    archivo.Read();
                    posicion++;
                    if(c == '\n')
                        linea++;
                    if (estado >0)
                        buffer += c;
                    else
                        buffer = "";
                }
            }
            setContenido(buffer); 
            if(estado == E)
                throw new Error("Error lexico: No definido en linea: "+linea, log);
            if(!FinArchivo())
                log.WriteLine(getContenido()+"\t"+getClasificacion());
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}
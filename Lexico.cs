// Fernando Hernández Domínguez
using System;
using System.IO;

namespace Generador{
    public class Lexico : Token{
        protected StreamReader archivo;
        protected StreamWriter log;
        const int F = -1;
        const int E = -2;
        protected int posicion;
        protected int linea;
        int[,] trand = new int[,]
        {
            
        };
        public Lexico(){
            linea = 1;
            log = new StreamWriter("C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Semantica\\c.log");
            log.AutoFlush = true;
            log.WriteLine("Archivo: c.gram");
            log.WriteLine("Compilado: " +DateTime.Now);
            if(File.Exists("C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Semantica\\c.gram"))
                archivo = new StreamReader("C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Semantica\\c.gram"); 
            else
                throw new Error("Error: El archivo c.gram no existe.", log);
        }

        public Lexico(String ruta){
            linea = 1;
            if(ruta == "C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Semantica\\c.gram")
                log = new StreamWriter("C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Semantica\\c.log");
            else{
                String nruta = Path.GetFileNameWithoutExtension(ruta);
                log = new StreamWriter(nruta +".log");
            }
            log.AutoFlush = true;
            log.WriteLine("Archivo: c.gram");
            log.WriteLine("Compilado: " +DateTime.Now);
            if(File.Exists(ruta))
                archivo = new StreamReader(ruta); 
            else
                throw new Error("Error: El archivo " +ruta +" no existe.", log);
        }
        public void cerrar(){
            archivo.Close();
            log.Close();
        }

        private void clasifica(int estado){
            
        }
        private int columna(char c){
            return 0;
        }
        public void NextToken(){
            string buffer = "";
            char c;
            int estado = 0;
            while(estado >= 0){
                c = (char)archivo.Peek();  // Funcion de transicion
                estado = trand[estado,columna(c)];
                clasifica(estado);
                if (estado >= 0){
                    if (c == '\n')
                        linea++;
                    if (estado > 0)
                        buffer += c;
                    else
                        buffer = "";
                }
            }
            setContenido(buffer);
            if(estado == E){
                throw new Error("Error Lexico no definido en linea : " +linea +".", log);
            }
        }

        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}
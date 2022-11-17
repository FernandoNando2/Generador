// Fernando Hernández Domínguez
using System;
namespace Generador{
    public class Program{
        static void Main(string[] args){
            using(Lenguaje a = new Lenguaje("C:\\Users\\Fernando Hernandez\\Desktop\\ITQ\\5to Semestre\\Lenguajes y Automatas II\\Generador\\c2.gram")){
                try{
                    a.gramatica();
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
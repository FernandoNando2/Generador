// Fernando Hernández Domínguez
using System;

namespace Generador{
    public class Program{

        static void Main(string[] args){
            using(Lenguaje a = new Lenguaje()){
                try{
                    
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
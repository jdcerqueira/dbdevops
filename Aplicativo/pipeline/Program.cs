using pipeline_core;
using pipeline_core_ControlDBDevops;
using System;

namespace pipeline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Pipeline(new Configuracao());
        }
    }
}

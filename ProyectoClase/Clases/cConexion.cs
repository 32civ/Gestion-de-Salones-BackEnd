using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ProyectoClase.Clases
{
    class cConexion
    {
        //Se define la ruta de la base de datos
        static private string CadenaConexion = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\source\repos\ProyectoClase\dbVeterinaria.mdf;Integrated Security=True;Connect Timeout=30";

        //Definir una variable para carhar la base de datos

        private SqlConnection Coenxion = new SqlConnection(CadenaConexion);

        //Metodo para abrir la base de datos
        public SqlConnection AbrirConexion()
        {
            if (Coenxion.State == ConnectionState.Closed)
            {
                Coenxion.Open();
            }
            return Coenxion;
        }

        //Metodo para cerrar la base de datos
        public SqlConnection CerrarConexion()
        {
            if (Coenxion.State == ConnectionState.Open)
            {
                Coenxion.Close();
            }
            return Coenxion;
        }

    }
}

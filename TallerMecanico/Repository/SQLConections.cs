using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TallerMecanico.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TallerMecanico.Repository
{
    public class SQLConections
    {
        SqlConnection conn;
        SqlCommand cmd;
        ConnectionString connectionString;
        string connection;

        public SQLConections()
        {
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                connectionString = JsonConvert.DeserializeObject<ConnectionString>(json);
                connection = "Data Source=" + connectionString.Server +
                             ";Initial Catalog=" + connectionString.Database +
                             ";User Id=" + connectionString.Uid +
                             ";Password=" + connectionString.Pwd;
            }
        }

        public DataTable getAutomovil(int id)
        {

            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectAutomovil " + id;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Automoviles");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var automovil = reader.Cast<Automovil>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable getAutomoviles()
        {

            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectAutomoviles";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Automoviles");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var automovil = reader.Cast<Automovil>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }


        public DataTable newAutomovil(string marca, string modelo, string patente, int tipo, int puertas)
        {

            conn = new SqlConnection(connection);
            string queryStatement = "EXEC NewAutomovil '" + marca + "','" + modelo + "','" + patente + "','" + tipo + "','" + puertas + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Automoviles");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();

            return customerTable;

        }

        public DataTable editAutomovil(int id, string marca, string modelo, string patente, int tipo, int puertas)
        {
            conn = new SqlConnection(connection);
            string queryStatement = "EXEC EditAutomovil " + id + ",'" + marca + "','" + modelo + "','" + patente + "','" + tipo + "','" + puertas + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Automoviles");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var automovil = reader.Cast<Automovil>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();

            return customerTable;
        }

        public DataTable getDesperfecto(int id)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectDesperfecto " + id;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Desperfecto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var repuestos = reader.Cast<Desperfecto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable getDesperfectos(int presupuestoId)
        {

            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectDesperfectos " + presupuestoId;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Desperfectos");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var repuestos = reader.Cast<Desperfecto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        internal DataTable editDesperfecto(int id, int idPresupuesto, string descripcion, float manoObra, int tiempo)
        {
            conn = new SqlConnection(connection);
            string queryStatement = "EXEC EditDesperfecto " + id + "," + idPresupuesto + ",'" + descripcion + "','" + manoObra + "','" + tiempo + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Desperfecto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var repuestos = reader.Cast<Desperfecto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();

            return customerTable;
        }

        public DataTable newDesperfecto(int idPresupuesto, string descripcion, float manoObra, int tiempo)
        {
            conn = new SqlConnection(connection);
            string queryStatement = "EXEC NewDesperfecto " + idPresupuesto + ",'" + descripcion + "','" + manoObra + "','" + tiempo + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Desperfecto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();

            return customerTable;
        }

        public DataTable getRepuestos(int desperfectoId)
        {

            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectRepuestos " + desperfectoId;

            cmd = new SqlCommand(queryStatement, conn);

            var customerTable = new DataTable("Repuestos");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var repuestos = reader.Cast<Repuesto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable getRepuestosSeleccionados(int desperfectoId)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectRepuestosEnDefecto " + desperfectoId;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Repuestos");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var desperfectoRepuesto = reader.Cast<DesperfectoRepuesto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable setRepuestosDesperfecto(int desperfectoId, int[] repuestosIds)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC EliminarRepuestoEnDefecto " + desperfectoId + "; ";

            foreach (int id in repuestosIds)
            {
                queryStatement += "EXEC SetRepuestoEnDefecto " + desperfectoId + ", " + id + "; ";
            }

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("RepuestosDesperfecto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var desperfectoRepuesto = reader.Cast<DesperfectoRepuesto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable getMoto(int idMoto)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectMoto " + idMoto;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Motos");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var motos = reader.Cast<Moto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable getMotos()
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectMotos";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Motos");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var motos = reader.Cast<Moto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable newMoto(string marca, string modelo, string patente, int cilindrada)
        {
            conn = new SqlConnection(connection);
            string queryStatement = "EXEC NewMoto '" + marca + "','" + modelo + "','" + patente + "','" + cilindrada + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Motos");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();

            return customerTable;
        }

        internal DataTable editMoto(int id, string marca, string modelo, string patente, int cilindrada)
        {
            conn = new SqlConnection(connection);
            string queryStatement = "EXEC EditAutomovil " + id + ",'" + marca + "','" + modelo + "','" + patente + "','" + cilindrada + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Motos");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var motos = reader.Cast<Moto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();

            return customerTable;
        }

        public DataTable getPresupusto(int vehiculoId)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC SelectPresupuesto " + vehiculoId;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Presupuesto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var presupuesto = reader.Cast<Presupuesto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        internal DataTable newPresupuesto(int vehiculoId, string nombre, string apellido, string email)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC NewPresupuesto " + vehiculoId + ",'" + nombre + "','" + apellido + "','" + email + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Presupuesto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable setPresupusto(int presupuestoId, string nombre, string apellido, string email)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC EditPresupuesto " + presupuestoId + ",'" + nombre + "','" + apellido + "','" + email + "'";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Presupuesto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var presupuesto = reader.Cast<Presupuesto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        internal DataTable editRepuesto(int repuestoId, string nombre, float precio)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC EditRepuesto " + repuestoId + ",'" + nombre + "'," + precio ;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Repuesto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            var repuesto = reader.Cast<Repuesto>();
            conn.Close();

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable newRepuesto(string nombre, float precio)
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC NewRepuesto '" + nombre + "'," + precio;

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Repuesto");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable resumen1()
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC ResumenMasUsado";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Resumen");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable resumen2()
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC ResumenPromedio";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Resumen");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }

        public DataTable resumen3()
        {
            conn = new SqlConnection(connection);

            string queryStatement = "EXEC ResumenSumatorias";

            cmd = new SqlCommand(queryStatement, conn);

            DataTable customerTable = new DataTable("Resumen");

            SqlDataAdapter _dap = new SqlDataAdapter(cmd);

            conn.Open();
            _dap.Fill(customerTable);
            conn.Close();
            return customerTable;
        }
    }
}

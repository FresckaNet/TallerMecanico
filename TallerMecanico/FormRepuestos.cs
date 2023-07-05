using System.Data;
using System.Data.SqlClient;
using TallerMecanico.Repository;

namespace TallerMecanico
{
    public partial class FormRepuestos : Form
    {
        private SQLConections sQLConections;
        public int desperfectoId;

        public FormRepuestos()
        {
            InitializeComponent();
            sQLConections = new SQLConections();
            listBox1.Items.Add("Repuestos seleccionados");
            listBox2.Items.Add("Repuestos en taller");
            listBox4.Items.Clear();
            listBox4.Items.Add("Cargando Repuesto ...");
            desperfectoId = 0;
            Shown += Load_Shown;
        }

        private void SetValuesToDataTableRepuestos(DataTable dataTable)
        {
            listBox4.Items.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                var repuestos = new string(string.Empty);
                repuestos += row.ItemArray[0].ToString() + "\t";
                repuestos += row.ItemArray[1].ToString() + "\t";
                repuestos += row.ItemArray[2].ToString();
                listBox4.Items.Add(repuestos);
            }
        }

        private void SetValuesToDataTableRepuestosSeleccionados(DataTable dataTable)
        {
            listBox3.Items.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                var repuestos = new string(string.Empty);
                repuestos += row.ItemArray[0].ToString() + "\t";
                repuestos += row.ItemArray[1].ToString() + "\t";
                repuestos += row.ItemArray[2].ToString();
                listBox3.Items.Add(repuestos);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem is not null)
            {
                listBox3.Items.Add(listBox4.SelectedItem);
                listBox4.Items.Remove(listBox4.SelectedItem);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem is not null)
            {
                listBox4.Items.Add(listBox3.SelectedItem);
                listBox3.Items.Remove(listBox3.SelectedItem);
            }
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            var repuestosIds = new int[listBox3.Items.Count];
            for (int i = 0; i < repuestosIds.Length; i++)
            {
                string[] parts = listBox3.Items[i].ToString().Split('\t');
                int id;
                if (int.TryParse(parts[0], out id))
                    repuestosIds[i] = id;
            }

            _ = sQLConections.setRepuestosDesperfecto(desperfectoId, repuestosIds);
            this.DialogResult = DialogResult.OK;
        }

        public void Load_Shown(object sender, EventArgs e)
        {
            this.Text += " Desperfecto: " + desperfectoId;
            DataTable repuestos = sQLConections.getRepuestos(desperfectoId);
            SetValuesToDataTableRepuestos(repuestos);
            DataTable repuestosSeleccionados = sQLConections.getRepuestosSeleccionados(desperfectoId);
            SetValuesToDataTableRepuestosSeleccionados(repuestosSeleccionados);
        }

    }
}

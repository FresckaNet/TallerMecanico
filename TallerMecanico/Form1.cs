using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TallerMecanico.Models;
using TallerMecanico.Repository;

namespace TallerMecanico
{
    public partial class Form1 : Form
    {
        private SQLConections sQLConections = new SQLConections();
        int vehiculoId, presupuestoId, desperfectoId, repuestoId;
        bool editar, automovil;

        public Form1()
        {
            InitializeComponent();
            hidegroupBoxs();
            listBox.Items.Add("Nro\tMarca\t\tModelo\t\tPatente\t\tTipo\t\tPuertas");
            listBox2.Items.Add("Nro\tPresupuesto\tMano de obra\t\tTiempo\t");
            listBox3.Items.Add("Repuestos selecciondos");
            comboBoxTipo.Items.Insert(0, Tipo.compacto);
            comboBoxTipo.Items.Insert(1, Tipo.sedan);
            comboBoxTipo.Items.Insert(2, Tipo.monovolumen);
            comboBoxTipo.Items.Insert(3, Tipo.utilitario);
            comboBoxTipo.Items.Insert(4, Tipo.lujo);
            numericUpDown.Minimum = 2;
            numericUpDown.Maximum = 6;
            textBox1.Enabled= false;
            textBox1.BackColor = Color.White;
            textBox1.ForeColor = Color.Black;
            textBox12.Enabled = false;
            textBox12.BackColor = Color.White;
            textBox12.ForeColor = Color.Black;
            textBox8.Enabled = false;
            textBox8.BackColor = Color.White;
            textBox8.ForeColor = Color.Black;
            Shown += buttonVehiculos_Click;
            DesabiliarTexboxesVehiculos();
            vehiculoId = presupuestoId = desperfectoId = repuestoId = 0;
            automovil = true;
            editar = false;
        }

        #region "Vehiculos"

        private void buttonVehiculos_Click(object sender, EventArgs e)
        {
            hidegroupBoxs();
            groupBoxVehiculos.Show();
            tableLayoutPanelMain.Controls.Add(groupBoxVehiculos, 1, 0);
            DesabiliarTexboxesVehiculos();
            if (automovil)
                buttonNuevo.Text = "+ Nuevo   \nAutomovil";
            else
                buttonNuevo.Text = "+ Nueva  \nMoto";
            
            buttonAddDefecto.Enabled = false;
           
            try
            {
                listBoxVehiculo.Items.Clear();
                listBoxVehiculo.Items.Add("Cargando vehiculos ...");
                DataTable automoviles = sQLConections.getAutomoviles();
                SetValuesToDataTableVehiculos(automoviles);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxVehiculo.Items.Clear();
                listBoxVehiculo.Items.Add("Error en carga de vehiculos.");
            }
        }

        private void listBoxVehiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] parts = ((ListBox)sender).SelectedItem.ToString().Split('.');
            int id;
            parts = parts.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (int.TryParse(parts[0].Trim('.'), out id))
             vehiculoId = id;
            
            textBox1.Text = "Espere...";
            textBox2.Text = "Cargando...";
            textBox6.Text = "Cargando...";
            textBox4.Text = "Cargando...";
            comboBoxTipo.Text = string.Empty;
            try
            {
                if (automovil)
                {
                    DataTable automoviles = sQLConections.getAutomovil(vehiculoId);

                    textBox1.Text = id.ToString();
                    textBox2.Text = automoviles.Rows[0][1].ToString();
                    textBox4.Text = automoviles.Rows[0][2].ToString();
                    textBox6.Text = automoviles.Rows[0][3].ToString();
                    Tipo t = (Tipo)Enum.Parse(typeof(Tipo), automoviles.Rows[0][4].ToString());
                    int numero = (int)t;
                    comboBoxTipo.SelectedIndex = numero;
                    numericUpDown.Value = Convert.ToInt32(automoviles.Rows[0][5].ToString());
                    labelVehiculoPresupuesto.Text = automoviles.Rows[0][1].ToString() + " " + automoviles.Rows[0][2].ToString();
                }
                else
                {
                    DataTable motos = sQLConections.getMoto(vehiculoId);

                    textBox1.Text = id.ToString();
                    textBox2.Text = motos.Rows[0][1].ToString();
                    textBox4.Text = motos.Rows[0][2].ToString();
                    textBox6.Text = motos.Rows[0][3].ToString();
                    numericUpDown.Value = Convert.ToInt32(motos.Rows[0][4].ToString());
                    labelVehiculoPresupuesto.Text = motos.Rows[0][1].ToString() + " " + motos.Rows[0][2].ToString();
                }

                DataTable presupuesto = sQLConections.getPresupusto(vehiculoId);
                listViewDesperfectos.Items.Clear();
                if (presupuesto.Rows.Count > 0)
                {
                    buttonAddDefecto.Enabled = false;
                    groupBoxPresupuesto.Text = "Presupuesto numero " + presupuesto.Rows[0][0].ToString();
                    presupuestoId = Convert.ToInt32(presupuesto.Rows[0][0]);
                    textBoxNombre.Text = presupuesto.Rows[0][1].ToString();
                    textBoxApellido.Text = presupuesto.Rows[0][2].ToString();
                    textBoxEmail.Text = presupuesto.Rows[0][3].ToString();
                    labelTotalPresupuesto.Text = "Total: $" + presupuesto.Rows[0][4].ToString();
                }
                else
                {
                    buttonAddDefecto.Enabled = true;
                    presupuestoId = 0;
                }

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DesabiliarTexboxesVehiculos();
            buttonEditar.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAutomovil.Checked)
            {
                automovil = true;
                buttonNuevo.Text = "+ Nuevo   \nAutomovil";
                listBox.Items[0] = ("Nro\tMarca\t\tModelo\t\tPatente\t\tTipo\t\tPuertas");
                label8.Text = "Puertas";
                numericUpDown.Minimum = 2;
                numericUpDown.Maximum = 6;
                vehiculoId = 0;
                ClearTexboxes();
                comboBoxTipo.Enabled = true;
                listBoxVehiculo.Items.Clear();
                listBoxVehiculo.Items.Add("Cargando vehiculos ...");
                try
                {
                    DataTable automoviles = sQLConections.getAutomoviles();
                    SetValuesToDataTableVehiculos(automoviles);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listBoxVehiculo.Items.Clear();
                    listBoxVehiculo.Items.Add("Error en carga de vehiculos.");
                }
            }
            else
            {
                automovil = false;
                buttonNuevo.Text = "+ Nueva  \nMoto";
                listBox.Items[0] = ("Nro\tMarca\t\tModelo\t\tPatente\t\tCilindrada");
                label8.Text = "Cilindrada";
                numericUpDown.Minimum = 10;
                numericUpDown.Maximum = 6000;
                vehiculoId = 0;
                ClearTexboxes();
                comboBoxTipo.Enabled = false;
                listBoxVehiculo.Items.Clear();
                listBoxVehiculo.Items.Add("Cargando vehiculos ...");
                try
                {
                    DataTable automoviles = sQLConections.getMotos();
                    SetValuesToDataTableVehiculos(automoviles);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listBoxVehiculo.Items.Clear();
                    listBoxVehiculo.Items.Add("Error en carga de vehiculos.");
                }
            }
        }


        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            editar = false;
            HabiliarTexboxesVehiculos();
            buttonEditar.Enabled = false;
            ClearTexboxes();
            textBox2.Focus();
            if(!automovil) comboBoxTipo.Enabled = false;
        }

        private void ClearTexboxes()
        {
            textBox1.Text = "-";
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
            comboBoxTipo.Text = string.Empty;
            if(automovil)
                numericUpDown.Value = 5;
            else 
                numericUpDown.Value = 10;
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            editar = true;
            HabiliarTexboxesVehiculos();
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (automovil)
                {
                    DataTable automoviles;
                    if (editar)
                    {
                        automoviles = sQLConections.editAutomovil(
                            vehiculoId, textBox2.Text, textBox4.Text, textBox6.Text, comboBoxTipo.SelectedIndex, (int)numericUpDown.Value);
                        SetValuesToDataTableVehiculos(automoviles);
                    }
                    else
                    {
                        automoviles = sQLConections.newAutomovil(
                            textBox2.Text, textBox4.Text, textBox6.Text, comboBoxTipo.SelectedIndex, (int)numericUpDown.Value);
                        SetValuesToDataTableVehiculos(automoviles);
                        listBoxVehiculo.SetSelected(listBoxVehiculo.Items.Count - 1, true);
                    }
                }
                else
                {
                    DataTable motos;
                    if (editar)
                    {
                        motos = sQLConections.editMoto(
                            vehiculoId, textBox2.Text, textBox4.Text, textBox6.Text, (int)numericUpDown.Value);
                        SetValuesToDataTableVehiculos(motos);
                    }
                    else
                    {
                        motos = sQLConections.newMoto(
                            textBox2.Text, textBox4.Text, textBox6.Text, (int)numericUpDown.Value);
                        SetValuesToDataTableVehiculos(motos);
                        listBoxVehiculo.SetSelected(listBoxVehiculo.Items.Count - 1, true);
                    }
                }
                MessageBox.Show("Vehiculo " + textBox2.Text + " " + textBox4.Text + " guardado", "Correcto !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            buttonGuardar.Enabled = false;
        }

        private void SetValuesToDataTableVehiculos(DataTable dataTable)
        {
            listBoxVehiculo.Items.Clear();
            if(dataTable.Rows.Count>0)

                foreach (DataRow row in dataTable.Rows)
                {
                    var vehiculo = new string(row.ItemArray[0] + ".\t");
                    for (int i = 1; i < 4; i++) vehiculo += LengthCheck(row.ItemArray[i].ToString());

                    if (automovil)
                    {
                        int tipo = Convert.ToInt32(row.ItemArray[4]);
                        vehiculo += LengthCheck(((Tipo)tipo).ToString());
                        vehiculo += row.ItemArray[5].ToString();
                    }
                    else
                        vehiculo += row.ItemArray[4].ToString();
                    listBoxVehiculo.Items.Add(vehiculo);
                }
            else
                listBoxVehiculo.Items.Add("\t\tLista de vehiculos vacia");
        }

        private void DesabiliarTexboxesVehiculos()
        {
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            comboBoxTipo.Enabled = false;
            comboBoxTipo.BackColor= Color.White;
            numericUpDown.ReadOnly = true;
            buttonGuardar.Enabled = false;
            buttonEditar.Enabled = false;
        }

        private void HabiliarTexboxesVehiculos()
        {
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox5.ReadOnly = false;
            textBox6.ReadOnly = false;
            comboBoxTipo.Enabled = true;
            numericUpDown.ReadOnly = false;
            buttonGuardar.Enabled = true;
        }

        #endregion


        #region "Desperfectos"

        private void buttonDesperfectos_Click(object sender, EventArgs e)
        {
            if (vehiculoId == 0)
            {
                MessageBox.Show("Seleccione un vehiculo primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (presupuestoId == 0)
            {
                MessageBox.Show("El vehiculo no tiene presupuesto.\nPuede crear uno.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            hidegroupBoxs();
            groupBoxDesperfectos.Show();
            tableLayoutPanelMain.Controls.Add(groupBoxDesperfectos, 1, 0);
            if (desperfectoId == -1) return;
            DesabiliarTexboxesDesperfecto();

            try
            {
                listBoxDesperfectos.Items.Clear();
                listBoxDesperfectos.Items.Add("Cargando desperfectos ...");
                DataTable desperfectos = sQLConections.getDesperfectos(presupuestoId);
                SetValuesToDataTableDesperfecto(desperfectos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxDesperfectos.Items.Clear();
                listBoxDesperfectos.Items.Add("Error en carga de desperfectos.");
            }
        }

        private void listBoxDesperfectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] parts = ((ListBox)sender).SelectedItem.ToString().Split('.');
            int id;
            parts = parts.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (int.TryParse(parts[0].Trim('.'), out id))
                desperfectoId = id;

            try
            {
                DataTable desperfecto = sQLConections.getDesperfecto(desperfectoId);

                textBox12.Text = id.ToString();
                textBox8.Text = desperfecto.Rows[0][1].ToString();
                textBox10.Text = desperfecto.Rows[0][2].ToString();
                numericUpDown2.Value = Convert.ToDecimal(desperfecto.Rows[0][3]);
                numericUpDown1.Value = Convert.ToInt32(desperfecto.Rows[0][4]);

                DataTable repuestos = sQLConections.getRepuestosSeleccionados(desperfectoId);
                SetValuesToDataTableRepuestos(repuestos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DesabiliarTexboxesDesperfecto();
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            editar = true;
            HabiliarTexboxesDesperfecto();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable desperfectos;
                if (editar)
                {
                    desperfectos = sQLConections.editDesperfecto(
                        desperfectoId, presupuestoId, textBox10.Text, (float)numericUpDown2.Value, (int)numericUpDown1.Value);
                    SetValuesToDataTableDesperfecto(desperfectos);
                }
                else
                {
                    desperfectos = sQLConections.newDesperfecto(
                        presupuestoId, textBox10.Text, (float)numericUpDown2.Value, (int)numericUpDown1.Value);
                    SetValuesToDataTableDesperfecto(desperfectos);
                    listBoxDesperfectos.SetSelected(listBoxDesperfectos.Items.Count - 1, true);
                }
                MessageBox.Show("Desperfecto \""+ textBox10.Text.Substring(0, 14) +"... \" guardado", "Correcto !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            buttonGuardar.Enabled = false;
        }

        private void SetValuesToDataTableDesperfecto(DataTable dataTable)
        {
            listBoxDesperfectos.Items.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                var desperfecto = new string(row.ItemArray[0].ToString() + ".\t");
                desperfecto += row.ItemArray[1].ToString() + "\t\t";
                desperfecto += row.ItemArray[2].ToString() + "\t\t";
                desperfecto += row.ItemArray[3].ToString();
                listBoxDesperfectos.Items.Add(desperfecto);
            }
        }

        private void DesabiliarTexboxesDesperfecto()
        {
            textBox12.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox10.ReadOnly = true;
            numericUpDown1.ReadOnly = true;
            numericUpDown2.ReadOnly = true;
            button4.Enabled = false;
            button3.Enabled = false;
        }

        private void HabiliarTexboxesDesperfecto()
        {
            textBox12.ReadOnly = false;
            textBox8.ReadOnly = false;
            textBox10.ReadOnly = false;
            numericUpDown1.ReadOnly = false;
            numericUpDown2.ReadOnly = false;
            button4.Enabled = true;
            button3.Enabled = true;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (desperfectoId != 0)
            {
                FormRepuestos form = new FormRepuestos();
                form.desperfectoId = desperfectoId;
                form.ShowDialog();
                SetValuesToDataTableRepuestos(sQLConections.getRepuestosSeleccionados(desperfectoId));
                
            }
        }

        private void buttonNuevoDesperfecto_Click(object sender, EventArgs e)
        {
            desperfectoId = -1;
            HabiliarTexboxesDesperfecto();
            editar = false;
            textBox12.Text = string.Empty;
            textBox8.Text = string.Empty;
            textBox10.Text = string.Empty;
            numericUpDown1.Value = 5;
            listBoxDesperfectos.Items.Clear();
            listBox4.Items.Clear();
            button4.Enabled = false;
            buttonDesperfectos_Click(null, new EventArgs());
        }

        #endregion


        #region "Presupuesto"

        private void buttonAddDefecto_Click(object sender, EventArgs e)
        {
            presupuestoId = -1;
            textBoxNombre.Text = string.Empty;
            textBoxApellido.Text = string.Empty;
            textBoxEmail.Text = string.Empty;
            listViewDesperfectos.Items.Clear();
            hidegroupBoxs();
            groupBoxPresupuesto.Show();
            tableLayoutPanelMain.Controls.Add(groupBoxPresupuesto, 1, 0);
        }

        private void SetValuesToDataTableRepuestos(DataTable dataTable)
        {
            listBox4.Items.Clear();
            listBoxRepuestos.Items.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                var repuestos = new string(string.Empty);
                repuestos += row.ItemArray[1].ToString() + "\t";
                repuestos += row.ItemArray[2].ToString();
                listBox4.Items.Add(repuestos);
                listBoxRepuestos.Items.Add(row.ItemArray[0].ToString() + "\t" + repuestos);
            }
            labelTotalRepuestos.Text = "Total " + dataTable.Rows.Count + " repuestos.";
        }

        private void textBoxCliente_TextChanged(object sender, EventArgs e)
        {
            buttonGuardarPresupuesto.Enabled = true;
        }

        private void buttonGuardarPresupuesto_Click(object sender, EventArgs e)
        {
            DataTable presupuesto;
            try
            {
                if (presupuestoId == -1)
                {
                    presupuesto = sQLConections.newPresupuesto(vehiculoId, textBoxNombre.Text, textBoxApellido.Text, textBoxEmail.Text);
                    presupuestoId = Convert.ToInt32(presupuesto.Rows[0].ItemArray[0]);
                    MessageBox.Show("Presupuesto creado con exito.", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    presupuesto = sQLConections.setPresupusto(presupuestoId, textBoxNombre.Text, textBoxApellido.Text, textBoxEmail.Text);
                    MessageBox.Show("Presupuesto actualizado.", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxVehiculo.Items.Clear();
                listBoxVehiculo.Items.Add("Error al actualizar de presupuestos.");
            }
        }

        private void buttonPresupuesto_Click(object sender, EventArgs e)
        {
            if (vehiculoId == 0)
            {
                MessageBox.Show("Seleccione un vehiculo primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (presupuestoId == 0)
            {
                MessageBox.Show("El vehiculo no tiene presupuesto.\nPuede crear uno.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            hidegroupBoxs();
            groupBoxPresupuesto.Show();
            tableLayoutPanelMain.Controls.Add(groupBoxPresupuesto, 1, 0);

            buttonGuardarPresupuesto.Enabled= false;

            try
            {
                DataTable desperfectos = sQLConections.getDesperfectos(presupuestoId);
                listViewDesperfectos.Items.Clear();
                if (desperfectos.Rows.Count > 0)
                {
                    foreach (DataRow row in desperfectos.Rows)
                    {
                        listViewDesperfectos.Items.Add(
                            "Nro " + row.ItemArray[0].ToString() + "\t$ " + row.ItemArray[2].ToString() + "\t"+ row.ItemArray[3].ToString() + " hs");
                    }
                }
                else
                    listViewDesperfectos.Items.Add("\t\tNo hay desperfectos cargados");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxVehiculo.Items.Clear();
                listBoxVehiculo.Items.Add("Error en carga de presupuestos.");
            }
        }

        #endregion


        #region "Repuesto"

        private void buttonRepuestos_Click(object sender, EventArgs e)
        {
            hidegroupBoxs();
            groupBoxRepuestos.Show();
            tableLayoutPanelMain.Controls.Add(groupBoxRepuestos, 1, 0);

            SetValuesToDataTableRepuestos(sQLConections.getRepuestos(0));
        }

        private void buttonNuevoRepuesto_Click(object sender, EventArgs e)
        {
            editar=false;
            repuestoId= 0;
            textBoxNombreRepuesto.Text=string.Empty;
            numericUpDownPrecioRepuesto.Value = 0;
        }

        private void buttonGuardarRepuestos_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable repuesto;
                if (editar)
                {
                    repuesto = sQLConections.editRepuesto(
                        repuestoId, textBoxNombreRepuesto.Text, (float)numericUpDownPrecioRepuesto.Value);
                    SetValuesToDataTableRepuestos(repuesto);
                }
                else
                {
                    repuesto = sQLConections.newRepuesto(
                        textBoxNombreRepuesto.Text, (float)numericUpDownPrecioRepuesto.Value);
                    SetValuesToDataTableRepuestos(repuesto);
                    listBoxRepuestos.SetSelected(listBoxRepuestos.Items.Count - 1, true);
                }
                MessageBox.Show(
                    "Repuesto \"" + textBoxNombreRepuesto.Text.Substring(0, 14) + "... \" guardado", "Correcto !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBoxRepuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            editar = true;
            string[] parts = listBoxRepuestos.SelectedItem.ToString().Split('\t');
            int id;
            if (int.TryParse(parts[0], out id))
                repuestoId = id;
            textBoxNombreRepuesto.Text = parts[1].ToString();
            numericUpDownPrecioRepuesto.Value = Convert.ToDecimal(parts[2]);
        }

        #endregion

        private void buttonResumen_Click(object sender, EventArgs e)
        {
            hidegroupBoxs();
            groupBoxResumen.Show();
            tableLayoutPanelMain.Controls.Add(groupBoxResumen, 1, 0);

            listBoxResumen.Items.Clear();
            listBoxResumen2.Items.Clear();
            listBoxResumen3.Items.Clear();

            try
            {
                DataTable resumen = sQLConections.resumen1();
                if (resumen.Rows.Count > 0)
                {
                    foreach (DataRow row in resumen.Rows)
                    {
                        listBoxResumen.Items.Add(
                            "En " + row.ItemArray[0].ToString() + " " + row.ItemArray[1].ToString() + ": \""
                            + row.ItemArray[2].ToString() + "  " + row.ItemArray[3].ToString() + " $" + row.ItemArray[4].ToString() + "\" "
                            + row.ItemArray[5].ToString() + " veces.");
                    }
                }

                resumen = sQLConections.resumen2();
                if (resumen.Rows.Count > 0)
                {
                    foreach (DataRow row in resumen.Rows)
                    {
                        listBoxResumen2.Items.Add(
                            "En " + row.ItemArray[0].ToString() + " " + row.ItemArray[1].ToString() + "= "
                            + "$" + row.ItemArray[2].ToString());
                    }
                }

                resumen = sQLConections.resumen3();
                if (resumen.Rows.Count > 0)
                {
                    foreach (DataRow row in resumen.Rows)
                    {
                        listBoxResumen3.Items.Add("En Automoviles: $" + row.ItemArray[0].ToString());
                        listBoxResumen3.Items.Add("En Motos: $" + row.ItemArray[1].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void hidegroupBoxs()
        {
            if(tableLayoutPanelMain.Controls.Count>1)
            {
                tableLayoutPanelMain.Controls.RemoveAt(1);
            }
            groupBoxVehiculos.Hide();
            groupBoxPresupuesto.Hide();
            groupBoxDesperfectos.Hide();
            groupBoxRepuestos.Hide();
            groupBoxResumen.Hide();
        }

        private string LengthCheck(string value) 
        {
            if (value.Length < 8) value += "\t";
            if (value.Length > 15)
            {
                value = value.Substring(0, 13);
                value += "..";
            }
            value += "\t";
            return value;
        }

    }
}
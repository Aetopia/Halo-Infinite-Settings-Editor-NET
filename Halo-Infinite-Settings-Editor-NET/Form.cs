using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

public class Form : System.Windows.Forms.Form
{
    public Form()
    {
        SpecControlSettings specControlSettings = new SpecControlSettings();
        this.Text = "Halo Infinite Settings Editor .NET";
        this.Font = SystemFonts.MessageBoxFont;
        this.MinimumSize = new Size(800, 600);
        this.CenterToScreen();

        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel() { Dock = DockStyle.Top, AutoSize = true };
        Panel panel = new Panel() { AutoSize = true, Dock = DockStyle.Fill };

        DataGridView dataGridView = new DataGridView()
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            MultiSelect = false,
            RowHeadersVisible = false,
            AllowUserToResizeRows = false,
            AllowUserToResizeColumns = false,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            SelectionMode = DataGridViewSelectionMode.CellSelect,
            ColumnHeadersVisible = false,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White
        };
        dataGridView.Columns.Add("Key", "Key");
        dataGridView.Columns["Key"].Resizable = DataGridViewTriState.False;
        dataGridView.Columns["Key"].ReadOnly = true;
        dataGridView.Columns["Key"].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.Columns.Add("Value", "Value");
        dataGridView.Columns["Value"].Resizable = DataGridViewTriState.False;
        dataGridView.Columns["Value"].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
        dataGridView.BorderStyle = BorderStyle.None;

        Button
        button1 = new Button() { Text = "Save", Margin = new Padding(0, 1, 0, 0) },
        button2 = new Button() { Text = "Refresh", Margin = new Padding(0, 1, 0, 0) };

        ComboBox comboBox = new ComboBox()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 1, 0, 0),
            AutoCompleteMode = AutoCompleteMode.Suggest,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };

        button1.Click += (sender, e) =>
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                string key = dataGridView.Rows[i].Cells[0].Value.ToString();
                try
                {
                    specControlSettings.jsonObject[key]["value"] =
                Convert.ChangeType(dataGridView.Rows[i].Cells[1].Value.ToString().Trim(), specControlSettings.jsonObject[key]["value"].GetType());
                }
                catch (FormatException) { }
            }
            specControlSettings.Write();
        };
        button2.Click += (sender, e) =>
        {
            comboBox.Items.Clear();
            dataGridView.Rows.Clear();
            foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in specControlSettings.jsonObject)
            {
                comboBox.Items.Add(keyValuePair.Key);
                dataGridView.Rows.Add(keyValuePair.Key, keyValuePair.Value["value"].ToString());
            }
            comboBox.SelectedIndex = 0;
            dataGridView.Select();
        };

        comboBox.KeyPress += (sender, e) => comboBox.DroppedDown = false;
        comboBox.LostFocus += (sender, e) => comboBox.SelectedIndex = dataGridView.CurrentCell.RowIndex;
        comboBox.SelectionChangeCommitted += (sender, e) =>
        {
            if (!comboBox.Focused)
                comboBox.Text = dataGridView.Rows[dataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString();
        }; comboBox.SelectedIndexChanged += (sender, e) =>
        {
            if (!dataGridView.Focused)
                dataGridView.CurrentCell = dataGridView.Rows[comboBox.SelectedIndex].Cells[1];
        };

        dataGridView.SelectionChanged += (sender, e) =>
        {
            if (dataGridView.CurrentCell.ColumnIndex == 0)
                dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentCell.RowIndex].Cells[1];
            if (!comboBox.Focused && comboBox.Items.Count != 0)
                comboBox.SelectedIndex = dataGridView.CurrentCell.RowIndex;
        };

        this.Resize += (sender, e) =>
        {
            if (!comboBox.Focused)
                comboBox.SelectionLength = 0;
        };
        this.Load += (sender, e) =>
        {
            panel.Padding = new Padding(0, tableLayoutPanel.ClientRectangle.Height, 0, 0);
            button2.PerformClick();
        };

        tableLayoutPanel.Controls.Add(button1, 0, 0);
        tableLayoutPanel.Controls.Add(button2, 1, 0);
        tableLayoutPanel.Controls.Add(comboBox, 2, 0);
        panel.Controls.Add(dataGridView);
        this.Controls.AddRange(new Control[] { tableLayoutPanel, panel });
    }
}
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;


public class SpecControlSettings
{
    private string filePath = "";
    private Dictionary<string, Dictionary<string, object>> jsonObject;

    public SpecControlSettings()
    {
        string
        localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA"),
        filePathSteam = $"{localAppData}\\HaloInfinite\\Settings\\SpecControlSettings.json",
        filePathUWP = $"{localAppData}\\Packages\\Microsoft.254428597CFE2_8wekyb3d8bbwe\\LocalCache\\Local\\HaloInfinite\\Settings\\SpecControlSettings.json";

        if (File.Exists(filePathSteam))
            this.filePath = filePathSteam;
        else if (File.Exists(filePathUWP))
            this.filePath = filePathUWP;
        this.GetSettings();
    }

    public Dictionary<string, string> GetSettings()
    {
        Dictionary<string, string> settings = new Dictionary<string, string>();
        this.jsonObject = (new JavaScriptSerializer()).Deserialize<Dictionary<string, Dictionary<string, object>>>(File.ReadAllText(this.filePath));
        foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in this.jsonObject)
            settings[keyValuePair.Key] = keyValuePair.Value["value"].ToString();
        return settings;
    }

    public void SetSettings(Dictionary<string, string> settings)
    {
        foreach (KeyValuePair<string, string> keyValuePair in settings)
        {
            this.jsonObject[keyValuePair.Key]["value"] = Convert.ChangeType(keyValuePair.Value.Trim(), this.jsonObject[keyValuePair.Key]["value"].GetType());
            File.WriteAllText(this.filePath, (new JavaScriptSerializer()).Serialize(jsonObject));
        }
    }

}

public class Form : System.Windows.Forms.Form
{
    private DataGridView dataGridView = new DataGridView()
    {
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
        Dock = DockStyle.Fill
    };
    private SpecControlSettings specControlSettings = new SpecControlSettings();
    private ToolStripComboBox keysComboBox = new ToolStripComboBox() { AutoCompleteMode = AutoCompleteMode.Suggest, AutoCompleteSource = AutoCompleteSource.ListItems };

    public Form()
    {
        this.Text = "Halo Infinite Settings Editor .NET";
        this.MinimumSize = new System.Drawing.Size(800, 600);
        this.CenterToScreen();
        this.AutoScaleMode = AutoScaleMode.Dpi;

        MenuStrip menuStrip = new MenuStrip();
        menuStrip.Dock = DockStyle.Top;

        ToolStripButton saveButton = new ToolStripButton("Save");
        saveButton.Click += (sender, e) =>
        {
            this.dataGridView.EndEdit();
            Dictionary<string, string> settings = new Dictionary<string, string>();
            for (int i = 0; i < dataGridView.RowCount; i++)
                settings[dataGridView.Rows[i].Cells[0].Value.ToString()] = dataGridView.Rows[i].Cells[1].Value.ToString();
            specControlSettings.SetSettings(settings);
        };

        ToolStripButton refreshButton = new ToolStripButton("Refresh");
        refreshButton.Click += (sender, e) =>
        {
            this.dataGridView.Rows.Clear();
            this.keysComboBox.Items.Clear();
            foreach (KeyValuePair<string, string> keyValuePair in specControlSettings.GetSettings())
            {
                this.keysComboBox.Items.Add(keyValuePair.Key);
                this.dataGridView.Rows.Add(keyValuePair.Key, keyValuePair.Value);
            }
            this.keysComboBox.Text = this.keysComboBox.Items[0].ToString();
            this.dataGridView.Select();
        };

        this.keysComboBox.KeyPress += (sender, e) => this.keysComboBox.DroppedDown = false;
        this.keysComboBox.LostFocus += (sender, e) => this.keysComboBox.Text = this.dataGridView.Rows[this.dataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString();
        this.keysComboBox.ComboBox.SelectionChangeCommitted += (sender, e) => this.keysComboBox.Text = this.dataGridView.Rows[this.dataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString();
        this.keysComboBox.SelectedIndexChanged += (sender, e) =>
        {
            if (!this.dataGridView.Focused)
            {
                this.dataGridView.CurrentCell = this.dataGridView.Rows[keysComboBox.SelectedIndex].Cells[1];
                this.dataGridView.Select();
            }
        };

        menuStrip.Items.Add(saveButton);
        menuStrip.Items.Add(refreshButton);
        menuStrip.Items.Add(this.keysComboBox);
        this.MainMenuStrip = menuStrip;

        Panel panel = new Panel();
        panel.Dock = DockStyle.Fill;
        panel.Padding = new Padding(0, menuStrip.Height + 3, 0, 0);
        panel.Controls.Add(dataGridView);
        this.Controls.AddRange(new Control[] { menuStrip, panel });

        dataGridView.Columns.Add("Key", "Key");
        dataGridView.Columns["Key"].Resizable = DataGridViewTriState.False;
        dataGridView.Columns["Key"].ReadOnly = true;
        dataGridView.Columns["Key"].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.Columns.Add("Value", "Value");
        dataGridView.Columns["Value"].Resizable = DataGridViewTriState.False;
        dataGridView.Columns["Value"].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
        dataGridView.BorderStyle = BorderStyle.None;
        dataGridView.SelectionChanged += (sender, e) =>
        {
            if (this.dataGridView.CurrentCell.ColumnIndex == 0)
                this.dataGridView.CurrentCell = this.dataGridView.Rows[this.dataGridView.CurrentCell.RowIndex].Cells[1];
            if (!keysComboBox.Focused)
                this.keysComboBox.Text = this.dataGridView.Rows[this.dataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString();
        };

        this.Load += (sender, e) =>
        {
            this.Resize += (sender, e) => { keysComboBox.Size = new Size(menuStrip.DisplayRectangle.Width - (saveButton.Size.Width + refreshButton.Size.Width + 7), -1); };
            this.OnResize(null);
            refreshButton.PerformClick();
        };
    }

}

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.Run(new Form());
    }
}

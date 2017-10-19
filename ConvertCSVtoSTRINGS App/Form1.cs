using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertCSVtoSTRINGS_App
{
    public partial class Form1 : Form
    {
        DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            dataTable = new DataTable();
            dataGridView1.DataSource = dataTable;
            dataTable.Columns.Add("Имя CSV");
            dataTable.Columns.Add("Имя будущего STRING");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (string file in Program.Args)
                if (CheckIfCSV(file) && !CheckAlreadyExists(file))
                    dataTable.LoadDataRow((String[])new ArrayList() { file, ConvertCSVtoSTRINGext(file) }.ToArray(typeof(string)), true);
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                if (CheckIfCSV(file) && !CheckAlreadyExists(file))
                    dataTable.LoadDataRow((String[])new ArrayList() { file, ConvertCSVtoSTRINGext(file) }.ToArray(typeof(string)), true);
        }

        string ConvertCSVtoSTRINGext(string file)
        {
            string a = file;

            a = a.Remove(a.Length - 4, 4);
            int dotpos = a.LastIndexOf('_');
            a = a.Remove(dotpos,1);
            a = a.Insert(dotpos, ".");
            //a += "STRINGS";

            return a;
        }

        bool CheckIfCSV(string file)
        {
            return (file.Substring(file.Length - 3).Equals("csv") ||
                file.Substring(file.Length - 3).Equals("Csv") ||
                file.Substring(file.Length - 3).Equals("CSV"));
        }

        bool CheckAlreadyExists(string file)
        {
            bool flag = false;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.Equals(file))
                    flag = true;
            }

            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String exePath = "StringsPacker.exe";

            ArrayList args = new ArrayList();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                String line = "";
                line += "\"" + dataGridView1.Rows[i].Cells[0].Value + "\"" + " ";
                line += "\"" + dataGridView1.Rows[i].Cells[1].Value + "\"" + " ";
                line += "/E1251";
                args.Add(line);
            }

            foreach (string line in args)
            {
                System.Diagnostics.Process.Start(exePath, line);
            }

        }
    }
}

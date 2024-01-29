using BL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProject
{
    public partial class Form1 : Form
    {
        private readonly FileModificationService _fileModificationService;
        private readonly DataComparisonService _dataComparisonService;
        public Form1()
        {
            InitializeComponent();
            _fileModificationService = new FileModificationService();
            _dataComparisonService = new DataComparisonService();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml file|*.xml|Csv file|*.csv";
            openFileDialog.Title = "Выбирите файл";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName == "") return;

            Task.Run(() =>
            {
                _fileModificationService.CopyFile(openFileDialog.FileName);
                var n = _dataComparisonService.Compare(openFileDialog.FileName);
                label1.Invoke((Action)(() => { label1.Text = $"Найдено совпадений {n}"; }));

                if (n > 0)  button2.Invoke((Action)(() => { button2.Enabled = true; }));
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            
            Task.Run(() =>
            {
                _dataComparisonService.GenerateReport();
                label1.Invoke((Action)(() => { label1.Text = $"Отчет сформирован"; }));
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShifrovanieOpenKey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int p, q, fi, n, d, eps, S;
        int[] C, M, M2, H;
        string text;

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(5);
            dataGridView1.Rows[0].Cells[0].Value = "p";
            dataGridView1.Rows[1].Cells[0].Value = "q";
            dataGridView1.Rows[2].Cells[0].Value = "M[]";
            dataGridView1.Rows[3].Cells[0].Value = "H[0]";
            dataGridView1.Rows[4].Cells[0].Value = "M[]";

            dataGridView1.Rows[0].Cells[1].Value = 17;
            dataGridView1.Rows[1].Cells[1].Value = 19;
            dataGridView1.Rows[2].Cells[1].Value = "МДВ";
            dataGridView1.Rows[3].Cells[1].Value = 7;
            dataGridView1.Rows[4].Cells[1].Value = "МТУСИ";
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index > -1)
            {
                string itemS = (sender as ListBox).Items[e.Index].ToString();
              Brush textBrush = Brushes.Black;
                if ((Convert.ToDouble(itemS) % 1) == 0)
                {
                    //textBrush = Brushes.Red;
                    e.Graphics.DrawRectangle(Pens.Red, e.Bounds);
                }
                else
                {
                    //textBrush = Brushes.Red;
                }
                e.Graphics.DrawString(itemS, e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                text = null;
                p = Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value);
                q = Convert.ToInt32(dataGridView1.Rows[1].Cells[1].Value);

                fi = (p - 1) * (q - 1);
                n = p * q;

                textBox1.Text = "f(n) = (p - 1) * (q - 1) = " + (p - 1) + " * " + (q - 1) + " = " + fi + "\r\n";
                textBox1.Text += "n = p * q = " + p + " * " + q + " = " + n + "\r\n";

                listBox1.Items.Clear();
                bool check;
                for (int i = 2; i < fi; i++)
                {
                    check = true;
                    for (int k = 2; k <= i; k++) {
                        if (((fi % k) == 0) && ((i % k)==0))
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        listBox1.Items.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                d = Convert.ToInt32(listBox1.SelectedItem);
                textBox1.Text += "d = " + d + "\r\n";
                listBox2.Items.Clear();
                for (int i = 1; i < fi; i++)
                {
                    listBox2.Items.Add((double)((fi * i) + 1) / d);
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //RSA
                string alfavit = "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя";
                eps = Convert.ToInt32(listBox2.SelectedItem);
                textBox1.Text += "e = " + eps + "\r\n";
                text = Convert.ToString(dataGridView1.Rows[2].Cells[1].Value);
                textBox1.Text += "\r\n" + text + " = (";
                M = new int[text.Length];
                for (int i = 0; i < M.Length; i++)
                {
                    M[i] = alfavit.IndexOf(text.Substring(i, 1)) / 2 + 1;
                    textBox1.Text += M[i] + ", ";
                }
                textBox1.Text = textBox1.Text.Substring(0, (textBox1.Text.Length - 2)) + ")\r\n";

                C = new int[text.Length];
                for (int i = 0; i < C.Length; i++)
                {
                    C[i] = mod(M[i], eps, n);
                    textBox1.Text += "C[" + i + "] = (" + M[i] + "^" + eps + ") mod " + n + " = " + C[i] + "\r\n";
                }

                M2 = new int[text.Length];
                for (int i = 0; i < M2.Length; i++)
                {
                    M2[i] = mod(C[i], d, n);
                    textBox1.Text += "M2[" + i + "] = (" + C[i] + "^" + d + ") mod " + n + " = " + M2[i] + "\r\n";
                }
                //hash
                text = Convert.ToString(dataGridView1.Rows[4].Cells[1].Value);
                textBox1.Text += "\r\n" + text + " = (";
                M = new int[text.Length];
                for (int i = 0; i < M.Length; i++)
                {
                    M[i] = alfavit.IndexOf(text.Substring(i, 1)) / 2 + 1;
                    textBox1.Text += M[i] + ", ";
                }
                textBox1.Text = textBox1.Text.Substring(0, (textBox1.Text.Length - 2)) + ")\r\n";

                H = new int[text.Length + 1];
                H[0] = (int)dataGridView1.Rows[3].Cells[1].Value;
                for (int i = 1; i < H.Length; i++)
                {
                    H[i] = mod(H[i - 1] + M[i-1], 2, n);
                    textBox1.Text += "H[" + i + "] = (("+H[i-1] + " + "+ M[i-1] + ")^2) mod " + n + " = " + H[i] + "\r\n";
                }
                //подпись
                S = mod(H[H.Length - 1], d, n);
                textBox1.Text += "\r\nS = (" + H[H.Length - 1] + "^" + d + ") mod " + n + " = " + S + "\r\n";
                textBox1.Text += "H = (" + S + "^" + eps + ") mod " + n + " = " + mod(S, eps, n) + "\r\n";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private int mod(int delimoe, int stepen, int delitel)
        {
            int[] arr = new int[Int16.MaxValue];
            // = (delimoe^stepen) mod delitel
            arr[0] = (delimoe % delitel);
            for (int k = 1; ; k++)
            {
                arr[k] = (delimoe * arr[k - 1]) % delitel;
                if (arr[k] == arr[0])
                {
                    return arr[(stepen % k) - 1];
                }
            }
        }
    }
}
